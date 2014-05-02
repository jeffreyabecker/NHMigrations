using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class AlterTableAddUniqueConstriantOperation : IOperation
    {
        public AlterTableAddUniqueConstriantOperation(UniqueKey uk)
        {

        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}