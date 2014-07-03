using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMigration.Model
{
    public class CreateSchemaOperation : IOperation
    {
        public string Name { get; set; }
    }
}
