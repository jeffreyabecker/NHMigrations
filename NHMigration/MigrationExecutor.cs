using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NHibernate;
using NHibernate.AdoNet.Util;
using NHibernate.Dialect;
using NHMigration.Model;
using NHMigration.Operations;
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

        public void Execute(Dialect dialect, TextWriter output, ISessionFactory sessionFactory)
        {
            var statements = GetStatements(dialect).ToList();
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


        private IEnumerable<string> GetStatements(Dialect dialect)
        {
            var fmt = new DdlFormatter();
            var ensureDbStatements = _versioningStrategy.EnsureDbObjectsStatements
                    .Select(st => st.Trim())
                        .Where(st => !String.IsNullOrWhiteSpace(st))
                        .Select(st => fmt.Format(st));
            foreach (var s in ensureDbStatements)
            {
                yield return s;
            }
            foreach (var migration in _migrations)
            {
                var opstatements = migration.GetOperations()
                    .Select(operation => operation.GetStatements(dialect)
                        .Select(st => st.Trim())
                        .Where(st => !String.IsNullOrWhiteSpace(st))
                        .Select(st => fmt.Format(st)))
                    .SelectMany(os => os);


                var updateVersionStatements = _versioningStrategy
                    .GetUpdateVersionStatements(migration.Version, migration.Context)
                        .Select(st => st.Trim())
                        .Where(st => !String.IsNullOrWhiteSpace(st))
                        .Select(st => fmt.Format(st));

                var migrationStatements = opstatements.Concat(updateVersionStatements);
     

                foreach (var s in opstatements)
                {
                    yield return s;
                }
                foreach (var s in updateVersionStatements)
                {
                    yield return s;
                }
            }
           
        }
    }
}