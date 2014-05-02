using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class CreateTableOperation : IOperation
    {
        public Table Table { get; set; }

        public CreateTableOperation() { }
        public CreateTableOperation(Table table)
        {
            Table = table;
        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}