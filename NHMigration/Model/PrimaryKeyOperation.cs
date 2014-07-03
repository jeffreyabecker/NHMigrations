using System.Collections.Generic;

namespace NHMigration.Model
{
    /// <summary>
    ///     Common base class to represent operations affecting primary keys.
    /// </summary>
    public abstract class PrimaryKeyOperation : IOperation
    {
        /// <summary>
        ///     Gets or sets the name of the table that contains the primary key.
        /// </summary>
        public TableModel Table { get; set; }

        /// <summary>
        ///     Gets the column(s) that make up the primary key.
        /// </summary>
        public IList<string> Columns { get; set; }


        /// <summary>  Gets or sets the name of this primary key.  If no name is supplied, a default name will be calculated.</summary>
        public string Name { get; set; }


        public string DefaultName
        {
            get { return BuildDefaultName(Table); }
        }


        public static string BuildDefaultName(TableModel table)
        {
            return "PK_" + table.Name;
        }
    }
}