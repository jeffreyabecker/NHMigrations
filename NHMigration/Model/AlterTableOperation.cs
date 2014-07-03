using System.Collections.Generic;
using System.Text;
using System.Threading;
using NHibernate.Dialect;

namespace NHMigration.Model
{
    /// <summary>Represents changes made to custom annotations on a table.</summary>
    public class AlterTableOperation : IOperation
    {
        /// <summary>
        ///     Gets the name of the table on which annotations have changed.
        /// </summary>
        public virtual TableModel Table { get; set; }

        /// <summary>
        ///     Gets the columns to be included in the table for which annotations have changed.
        /// </summary>
        public virtual IList<ColumnModel> Columns { get; set; }

        
    }
}