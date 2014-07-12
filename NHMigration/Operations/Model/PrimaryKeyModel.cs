using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping;

namespace NHMigration.Operations.Model
{
    public class PrimaryKeyModel
    {
        public List<Column> PkCols { get; set; }
        public bool Identity { get; set; }
        public string Name { get; set; }
        public bool Clustered { get; set; }


        public PrimaryKeyModel(List<Column> pkCols, bool identity, string name, bool clustered)
        {
            PkCols = pkCols;
            Identity = identity;
            Name = name;
            Clustered = clustered;
        }
    }
}
