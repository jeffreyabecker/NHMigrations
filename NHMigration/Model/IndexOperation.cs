using System;
using System.Collections.Generic;

namespace NHMigration.Model
{
    /// <summary>
    ///     Common base class for operations affecting indexes.
    /// </summary>
    public abstract class IndexOperation : IOperation
    {
        /// <summary>
        ///     Gets or sets the table the index belongs to.
        /// </summary>
        public TableModel Table { get; set; }

        /// <summary>
        ///     Gets the columns that are indexed.
        /// </summary>
        public IList<string> Columns { get; set; }


        /// <summary>
        ///     Gets or sets the name of this index.
        ///     If no name is supplied, a default name will be calculated.
        /// </summary>
        public string Name { get; set; }

        public string DefaultName
        {
            get { return BuildDefaultName(Columns); }
        }


        /// <summary>Creates a default index name based on the supplied column names.</summary>
        /// <param name="columns">The column names used to create a default index name.</param>
        /// <returns>A default index name.</returns>
        public static string BuildDefaultName(IEnumerable<string> columns)
        {
            return "IX_" + String.Join("_", columns);
        }
    }
}