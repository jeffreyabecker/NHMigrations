using System;
using System.Collections.Generic;
using NHMigration.Model;
using NHMigration.Operations;

namespace NHMigration
{
    public interface IMigration
    {
        IEnumerable<IOperation> GetOperations();
        string Version { get; }
        string Context { get;}
    }
}
