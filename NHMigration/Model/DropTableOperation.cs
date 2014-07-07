using System.Collections.Generic;

namespace NHMigration.Model
{
    /// <summary>
    ///     Represents dropping an existing table.
    /// </summary>
    public class DropTableOperation : IOperation

    {
        public DropTableOperation(){}

        public DropTableOperation(NHibernate.Mapping.Table table)
        {
            this.Table = Table;
        }
        
        public virtual NHibernate.Mapping.Table Table { get; set; }
        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;
            var drop = dialect.GetDropTableString(Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema));
            return new[] {new MigrationStatement(drop),};
        }

        public IOperation Inverse
        {
            get
            {
                return new CreateTableOperation(Table);
            }
        }
    }
}