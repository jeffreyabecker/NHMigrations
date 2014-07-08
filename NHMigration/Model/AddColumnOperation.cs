using System.Collections.Generic;

namespace NHMigration.Model
{
    /// <summary>
    ///     Represents a column being added to a table.
    /// </summary>
    public class AddColumnOperation : IOperation
    {
        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultSchema = context.DefaultSchema;
            var defaultCatalog = context.DefaultCatalog;

            throw new System.NotImplementedException();
        }

        public IOperation Inverse { get; private set; }
    }

    /// <summary>
    ///     Represents a column being dropped from a table.
    /// </summary>
    public class DropColumnOperation
    {

    }

    /// <summary>Represents altering an existing column.</summary>
    public class AlterColumnOperation : IOperation
    {
        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultSchema = context.DefaultSchema;
            var defaultCatalog = context.DefaultCatalog;

            
            throw new System.NotImplementedException();
        }

        public IOperation Inverse { get; private set; }
    }
}