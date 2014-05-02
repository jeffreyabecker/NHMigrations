using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class AlterTableAlterColumnOperation : IOperation
    {
        public AlterTableAlterColumnOperation(MigrationGenerator.AlteredTable alteredTable, Column column)
        {

        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}