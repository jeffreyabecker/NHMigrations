using System.Collections.Generic;
using NHMigration.Operations;

namespace NHMigration
{
    public interface IMigration
    {
        IEnumerable<IOperation> Operations { get; }
        string Version { get; }
        string Context { get;}
    }
}
