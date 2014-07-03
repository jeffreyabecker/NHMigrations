namespace NHMigration.Model
{
    /// <summary>
    ///     Represents creating a database index.
    /// </summary>
    public class CreateIndexOperation : IndexOperation
    {
        /// <summary>
        ///     Gets or sets a value indicating if this is a unique index.
        /// </summary>
        public bool IsUnique { get; set; }


        /// <summary>
        ///     Gets or sets whether this is a clustered index.
        /// </summary>
        public bool IsClustered { get; set; }
    }
}