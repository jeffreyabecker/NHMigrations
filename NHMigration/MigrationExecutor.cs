using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NHibernate;
using NHMigration.Model;
using NHMigration.Versioning;

namespace NHMigration
{
    public class MigrationExecutor
    {
        private readonly IEnumerable<IMigration> _migrations;
        private readonly IVersioningStrategy _versioningStrategy;

        public MigrationExecutor(IEnumerable<IMigration> migrations, IVersioningStrategy versioningStrategy)
        {
            _migrations = migrations;
            _versioningStrategy = versioningStrategy;
        }

        public void Execute(IMigrationContext context,  TextWriter output, ISessionFactory sessionFactory)
        {
            var statements = GetStatements(context).ToList();
            foreach (var statement in statements)
            {
                output.WriteLine(statement);
            }
            using (var s = sessionFactory.OpenStatelessSession())
            {
                foreach (var statement in statements)
                {
                    using (var cmd = s.Connection.CreateCommand())
                    {
                        cmd.CommandText = statement;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }


        private IEnumerable<string> GetStatements(IMigrationContext context)
        {
            
            foreach (var s in _versioningStrategy.EnsureDbObjectsStatements)
            {
                yield return(s);
                yield return(context.Dialect.BatchTerminator);
            }
            foreach (var migration in _migrations)
            {
                foreach (var operation in migration.GetOperations())
                {
                    foreach (var s in operation.GetStatements(context))
                    {
                        yield return(s);
                        yield return(context.Dialect.BatchTerminator);
                    }
                }

                foreach (var s in _versioningStrategy.GetUpdateVersionStatements(migration.Version, migration.Context))
                {
                    yield return(s);
                    yield return(context.Dialect.BatchTerminator);
                }
            }
           
        }
    }
}