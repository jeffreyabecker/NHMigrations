using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMigration.Model.Extensions
{
    public static class MappingExtensions
    {
        public static bool HasDefaultValue(this NHibernate.Mapping.Column col)
        {
            return !String.IsNullOrWhiteSpace(col.DefaultValue);
        }

        public static StringBuilder AppendRange(this StringBuilder sb, IEnumerable<string> range)
        {
            foreach (var s in range)
            {
                sb.Append(s);
            }
            return sb;
        }
    }
}
