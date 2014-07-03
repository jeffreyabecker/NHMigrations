using System.Collections.Generic;
using System.Text;

namespace NHMigration.Model
{
    /// <summary>
    ///     Represents creating a table.
    /// </summary>
    public class CreateTableOperation : IOperation
    {
        /// <summary>
        ///     Gets the name of the table to be created.
        /// </summary>
        public virtual TableModel Name { get; set; }

        /// <summary>
        ///     Gets the columns to be included in the new table.
        /// </summary>
        public virtual IList<ColumnModel> Columns { get; set; }

        /// <summary>
        ///     Gets or sets the primary key for the new table.
        /// </summary>
        public AddPrimaryKeyOperation PrimaryKey { get; set; }

        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext ctx)
        {
            
            var sb = new StringBuilder();
            sb.Append(ctx.Dialect.CreateTableString)
                .Append(" ")
                .Append(Name.GetQualifiedName(ctx.Dialect, ctx.DefaultCatalog, ctx.DefaultSchema))
                .Append(" (");


        }
    }
}