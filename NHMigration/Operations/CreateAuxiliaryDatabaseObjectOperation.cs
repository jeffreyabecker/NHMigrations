using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class CreateAuxiliaryDatabaseObjectOperation : IOperation
    {
        public IAuxiliaryDatabaseObject AuxDbObj { get; set; }

        public CreateAuxiliaryDatabaseObjectOperation(IAuxiliaryDatabaseObject auxDbObj)
        {
            AuxDbObj = auxDbObj;
        }

        public void Apply(MigrationDatabaseContex ctx)
        {
            throw new NotImplementedException();
        }
    }
}