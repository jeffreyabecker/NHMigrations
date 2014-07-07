using NHibernate.Dialect;

namespace NHMigration.Model
{
    public interface IMigrationContext
    {
        Dialect Dialect { get; }
        string DefaultCatalog { get; }
        string DefaultSchema { get; }
    }
}