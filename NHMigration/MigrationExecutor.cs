using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NHibernate;
using NHibernate.AdoNet.Util;
using NHibernate.Dialect;
using NHMigration.Versioning;

namespace NHMigration
{
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

        public void WriteOutMigrationScript(Dialect dialect, TextWriter tw)
        {
            WriteOutMigrationScript(GenerateDataDefintionLanguage(dialect), tw);
        }

        public void ExecuteMigrationScript(Dialect dialect, TextWriter log = null)
        {
            log = log ?? new StringWriter();
            using (var s = _sessionFactory.OpenStatelessSession())
            {
                var conn = s.Connection;
                foreach (var statement in GenerateDataDefintionLanguage(dialect))
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
        public IEnumerable<string> GenerateDataDefintionLanguage(Dialect dialect)
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
                    foreach (var s in o.GetStatements(dialect))
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