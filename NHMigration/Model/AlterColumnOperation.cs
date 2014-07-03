namespace NHMigration.Model
{
    /// <summary>Represents altering an existing column.</summary>
    public class AlterColumnOperation : IOperation
    {
        /// <summary>Gets the name of the table that the column belongs to.</summary>
        public TableModel Table { get; set; }

        /// <summary>
        ///     Gets the new definition for the column.
        /// </summary>
        public ColumnModel Column { get; set; }
    }
}