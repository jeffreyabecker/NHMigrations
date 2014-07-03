namespace NHMigration.Model
{
    /// <summary>
    ///     Represents a column being dropped from a table.
    /// </summary>
    public class DropColumnOperation : IOperation
    {
        /// <summary>
        ///     Gets the name of the table the column should be dropped from.
        /// </summary>
        public TableModel Table { get; set; }

        /// <summary>
        ///     Gets the name of the column to be dropped.
        /// </summary>
        public string Name { get; set; }
    }
}