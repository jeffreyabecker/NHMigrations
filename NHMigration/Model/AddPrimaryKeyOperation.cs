namespace NHMigration.Model
{
    /// <summary>
    ///     Represents adding a primary key to a table.
    /// </summary>
    public class AddPrimaryKeyOperation : PrimaryKeyOperation
    {
        /// <summary>
        ///     Gets or sets whether this is a clustered primary key.
        /// </summary>
        public bool IsClustered { get; set; }
    }
}