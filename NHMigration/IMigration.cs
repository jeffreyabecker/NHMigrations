using System.Collections.Generic;
using NHMigration.Model;
using NHMigration.Versioning;

namespace NHMigration
{
    public interface IMigration
    {
        IEnumerable<IOperation> GetOperations();
        string Version { get; }
        string Context { get;}
    }

    public class MigrationExecutor
    {
        public MigrationExecutor(IMigrationContext context, IEnumerable<IMigration> migrations, IVersioningStrategy versioningStrategy)
        {
            var statements = new List<string>();
            statements.AddRange(versioningStrategy.EnsureDbObjectsStatements);
            foreach (var migration in migrations)
            {

                foreach (var operation in migration.GetOperations())
                {
                    statements.AddRange(operation.GetStatements(context));    
                }
                
                statements.AddRange(versioningStrategy.GetUpdateVersionStatements(migration.Version, migration.Context));
            }
                
        }
    }
}
