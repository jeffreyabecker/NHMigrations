using System;
using System.Collections.Generic;
using System.IO;
using NHibernate;
using NHMigration.Operations;
using NHMigration.Versioning;

namespace NHMigration
{
    public interface IMigration
    {
        IEnumerable<IOperation> Operations { get; }
        string Version { get; }
        string Context { get;}
    }

    public class MigrationExecutor
    {
        private readonly IEnumerable<IMigration> _migrations;
        private readonly IVersioningStrategy _versioningStrategy;
        private readonly ISessionFactory _sessionFactory;

        public MigrationExecutor(IEnumerable<IMigration> migrations, IVersioningStrategy versioningStrategy, ISessionFactory sessionFactory)
        {
            _migrations = migrations;
            _versioningStrategy = versioningStrategy;
            _sessionFactory = sessionFactory;

        }

        public void WriteOutMigrationScript(IMigrationContext ctx, TextWriter tw)
        {
            WriteOutMigrationScript(GenerateDataDefintionLanguage(ctx), tw);
        }

        public void ExecuteMigrationScript(IMigrationContext ctx, TextWriter log = null)
        {
            log = log ?? new StringWriter();
            using (var s = _sessionFactory.OpenStatelessSession())
            {
                var conn = s.Connection;
                foreach (var statement in GenerateDataDefintionLanguage(ctx))
                {
                    log.WriteLine(statement);
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = statement;
                        cmd.CommandType = CommandType.Text;
                    }
                }
            }
        }
        private void WriteOutMigrationScript(IEnumerable<string> statements, TextWriter tw)
        {
            foreach (var s in statements)
            {
                tw.WriteLine(s.TrimEnd());
            }
        }
        public IEnumerable<string> GenerateDataDefintionLanguage(IMigrationContext ctx)
        {
            var formatter = new DdlFormatter();
            
            foreach (var s in _versioningStrategy.EnsureDbObjectsStatements)
            {
                if (!String.IsNullOrWhiteSpace(s))
                {
                    yield return formatter.Format(s).TrimEnd();
 
                }
            }
            foreach (var m in _migrations)
            {
                foreach (var o in m.Operations)
                {
                    foreach (var s in o.GetStatements(ctx))
                    {
                        if (!String.IsNullOrWhiteSpace(s))
                        {
                            yield return formatter.Format(s).TrimEnd();
                        }                   
                    }
                }
                var updateVersionStatements = _versioningStrategy.GetUpdateVersionStatements(m.Version, m.Context);
                foreach (var s in updateVersionStatements)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        yield return formatter.Format(s).TrimEnd();
                    }
                }
            }
        }
    }

}
