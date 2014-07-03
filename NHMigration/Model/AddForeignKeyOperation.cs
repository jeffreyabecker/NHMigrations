using System.Collections.Generic;

namespace NHMigration.Model
{
    /// <summary>
    ///     Represents a foreign key constraint being added to a table.
    /// </summary>
    public class AddForeignKeyOperation : ForeignKeyOperation
    {
        public IList<string> PrincipalColumns { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating if cascade delete should be configured on the foreign key constraint.
        /// </summary>
        public bool CascadeDelete { get; set; }
    }
}