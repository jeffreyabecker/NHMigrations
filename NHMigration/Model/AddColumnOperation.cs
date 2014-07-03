namespace NHMigration.Model
{
    /// <summary>
    ///     Represents a column being added to a table.
    /// </summary>
    public class AddColumnOperation : IOperation
    {
        
        /// <summary>
        ///     Gets the name of the table the column should be added to.
        /// </summary>
        public TableModel Table { get; set; }

        /// <summary>
        ///     Gets the details of the column being added.
        /// </summary>
        public ColumnModel Column { get; set; }
    }
}