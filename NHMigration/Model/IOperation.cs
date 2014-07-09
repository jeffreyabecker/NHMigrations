using System.Collections.Generic;

namespace NHMigration.Model
{
    public interface IOperation
    {
        IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context);
        //IOperation Inverse { get; }
    }
}
