using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHMigration.Versioning;

namespace NHMigration.Operations
{
    public interface IOperation
    {
        void Apply(MigrationDatabaseContex ctx);
    }
}
