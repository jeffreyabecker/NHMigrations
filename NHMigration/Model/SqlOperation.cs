using System.Collections.Generic;
using NHibernate.Dialect;

namespace NHMigration.Model
{
    /// <summary>
    ///     Represents a provider specific SQL statement to be executed directly against the target database.
    /// </summary>
    public class SqlOperation : IOperation
    {
        /// <summary>
        ///     Gets the SQL to be executed.
        /// </summary>
        public virtual string Sql { get; set; }

        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            return new MigrationStatementResult(Sql);
        }

        public IOperation Inverse { get; set; }
    }
}