using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class DropIndexOperation : IOperation
    {
        public DropIndexOperation(Index index)
        {

        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}