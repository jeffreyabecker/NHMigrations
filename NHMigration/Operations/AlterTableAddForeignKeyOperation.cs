using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class AlterTableAddForeignKeyOperation : IOperation
    {
        public AlterTableAddForeignKeyOperation(ForeignKey fk)
        {

        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}