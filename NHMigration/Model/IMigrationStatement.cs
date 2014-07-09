namespace NHMigration.Model
{
    public interface IMigrationStatement
    {
        string Sql { get; }
        string BatchTerminator { get; }
    }
}