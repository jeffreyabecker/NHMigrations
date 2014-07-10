using System.Data;
using System.Security.Cryptography.X509Certificates;
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