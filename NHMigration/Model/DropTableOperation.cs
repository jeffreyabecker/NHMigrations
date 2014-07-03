namespace NHMigration.Model
{
    /// <summary>
    ///     Represents dropping an existing table.
    /// </summary>
    public class DropTableOperation : IOperation
    {
        /// <summary>
        ///     Gets the name of the table to be dropped.
        /// </summary>
        public virtual TableModel Name { get; set; }
    }
}