using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class AlterTableDropColumnOperation : IOperation
    {
        public AlterTableDropColumnOperation(Column column)
        {


        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}