using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class AlterTableAddColumnOperation : IOperation
    {
        public AlterTableAddColumnOperation(MigrationGenerator.AlteredTable alteredTable, Column column)
        {

        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}