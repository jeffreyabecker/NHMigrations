using System.Collections.Generic;
using NHibernate.Dialect;

namespace NHMigration.Operations
{
    public interface IOperation
    {
        IEnumerable<string> GetStatements(Dialect dialect);
        //IOperation Inverse { get; }
    }
}
