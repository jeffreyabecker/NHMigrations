using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Dialect;

namespace NHMigration.Model
{
    public interface IMigrationStatement
    {
        string Sql { get; }
        string BatchTerminator { get; }
    }

    public class MigrationStatement : IMigrationStatement
    {
        public string Sql { get; set; }

        public string BatchTerminator { get; set; }
    }

    public interface IOperation
    {
        IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context);
    }

    public interface IMigrationContext
    {
        Dialect Dialect { get; }
        string DefaultCatalog { get; }
        string DefaultSchema { get; }
    }


}
