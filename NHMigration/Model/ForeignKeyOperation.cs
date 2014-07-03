using System;
using System.Collections.Generic;
using System.Globalization;

namespace NHMigration.Model
{
    /// <summary>
    ///     Base class for changes that affect foreign key constraints.
    /// </summary>
    public abstract class ForeignKeyOperation : IOperation
    {
        /// <summary>
        ///     Gets or sets the name of the table that the foreign key constraint targets.
        /// </summary>
        public TableModel PrincipalTable { get; set; }

        /// <summary>
        ///     Gets or sets the name of the table that the foreign key columns exist in.
        /// </summary>
        public TableModel DependentTable { get; set; }

        /// <summary>
        ///     The names of the foreign key column(s).
        /// </summary>
        public IList<string> DependentColumns { get; set; }


        /// <summary>
        ///     Gets or sets the name of this foreign key constraint. If no name is supplied, a default name will be
        ///     calculated.
        /// </summary>
        public string Name { get; set; }

        public string DefaultName
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "FK_{0}_{1}_{2}",
                    DependentTable, PrincipalTable, String.Join("_", DependentColumns));
            }
        }
    }
}