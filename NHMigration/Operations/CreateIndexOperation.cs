using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class CreateIndexOperation : IOperation
    {
        public CreateIndexOperation(Index index)
        {

        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}