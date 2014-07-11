using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping;
using NHMigration.Model;
using NHMigration.Operations.Extensions;

namespace NHMigration.Operations
{
    /// <summary>
    ///     Represents a column being added to a table.
    /// </summary>
    public class AddColumnOperation : IOperation
    {
        private readonly Table _table;
        private readonly Column _column;

        public AddColumnOperation(Table table, Column column)
        {
            _table = table;
            _column = column;
        }

        public IEnumerable<string> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultSchema = context.DefaultSchema;
            var defaultCatalog = context.DefaultCatalog;

            var tableName = _table.GetQualifiedName(dialect, defaultCatalog, defaultSchema);
            var sb = new StringBuilder();
            sb.Append("alter table ")
                .Append(tableName)
                .Append(" ")
                .Append(dialect.AddColumnString)
                .Append(" ");
            sb.Append(_column.GetQuotedName(dialect))
                .Append(" ")
                .Append(_column.GetSqlType(dialect))
                .Append(" ");
            if (_column.HasDefaultValue())
            {
                sb.Append(" default ").Append(_column.DefaultValue).Append(" ");
            }
            sb.Append(_column.IsNullable ? dialect.NullColumnString : " not null");

            bool useUniqueConstraint = _column.Unique && dialect.SupportsUnique
                               && (!_column.IsNullable || dialect.SupportsNotNullUnique);
            if (useUniqueConstraint)
            {
                sb.Append(" unique");
            }

            if (_column.HasCheckConstraint && dialect.SupportsColumnCheck)
            {
                sb.Append(" check(").Append(_column.CheckConstraint).Append(") ");
            }
            return new[] { sb.ToString() };
        }


    }
}