using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping;
using NHMigration.Model;
using NHMigration.Operations.Extensions;

namespace NHMigration.Operations
{
    /// <summary>Represents altering an existing column.</summary>
    public class AlterColumnOperation : IOperation
    {
        private readonly Table _table;
        private readonly Column _oldColumn;
        private readonly Column _newColumn;


        public AlterColumnOperation(Table table, Column oldColumn, Column newColumn)
        {
            _table = table;
            _oldColumn = oldColumn;
            _newColumn = newColumn;
        }

        public IEnumerable<string> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultSchema = context.DefaultSchema;
            var defaultCatalog = context.DefaultCatalog;
            if (!dialect.SupportsAlterColumn)
            {
                throw new NotSupportedException(String.Format("{0} does not support altering columns", dialect.GetType().Name));
            }
            var tableName = _table.GetQualifiedName(dialect, defaultCatalog, defaultSchema);
            var sb = new StringBuilder();
            sb.Append("alter table ")
                .Append(tableName)
                .Append(" ")
                .Append(dialect.AlterColumnString)
                .Append(" ");
            sb.Append(_newColumn.GetQuotedName(dialect))
                .Append(" ")
                .Append(_newColumn.GetSqlType(dialect))
                .Append(" ");
            if (_newColumn.HasDefaultValue())
            {
                sb.Append(" default ").Append(_newColumn.DefaultValue).Append(" ");
            }
            sb.Append(_newColumn.IsNullable ? dialect.NullColumnString : " not null");

            bool useUniqueConstraint = _newColumn.Unique && dialect.SupportsUnique
                                       && (!_newColumn.IsNullable || dialect.SupportsNotNullUnique);
            if (useUniqueConstraint)
            {
                sb.Append(" unique");
            }

            if (_newColumn.HasCheckConstraint && dialect.SupportsColumnCheck)
            {
                sb.Append(" check(").Append(_newColumn.CheckConstraint).Append(") ");
            }
            return new []{sb.ToString()};
        }

    }
}