using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Mapping;

namespace NHMigration.Operations.Extensions
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

        public static string GetSqlType(this Column column, Dialect dialect)
        {
            if (!String.IsNullOrWhiteSpace(column.SqlType))
            {
                return column.SqlType;
            }
            return dialect.GetTypeName(column.SqlTypeCode);
        }
    }
}
