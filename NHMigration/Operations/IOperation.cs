using System.Collections.Generic;

namespace NHMigration.Model
{
    public interface IOperation
    {
        IEnumerable<string> GetStatements(IMigrationContext context);
        //IOperation Inverse { get; }
    }
}
