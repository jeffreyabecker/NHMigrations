using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHMigration.Model;

namespace NHMigration.Operations
{
    /// <summary>
    ///     Represents a column being dropped from a table.
    /// </summary>
    public class DropColumnOperation : IOperation
    {
        //private readonly Table _table;
        //private readonly Column _column;

        //public DropColumnOperation(Table table, Column column)
        //{
        //    _table = table;
        //    _column = column;
        //}
        //public IEnumerable<string> GetStatements(IMigrationContext context)
        //{
        //    var dialect = context.Dialect;
        //    var defaultSchema = context.DefaultSchema;
        //    var defaultCatalog = context.DefaultCatalog;
        //    if (!dialect.SupportsDropColumn)
        //    {
        //        throw new NotSupportedException(String.Format("{0} does not support dropping columns", dialect.GetType().Name));
        //    }
        //    var tableName = _table.GetQualifiedName(dialect, defaultCatalog, defaultSchema);
        //    var sb = new StringBuilder();
        //    sb.Append("alter table ")
        //        .Append(tableName)
        //        .Append(" ")
        //        .Append(dialect.DropColumnString)
        //        .Append(" ")
        //        .Append(_column.GetQuotedName(dialect));

        //    return new[]{sb.ToString()};
        //}

        public IEnumerable<string> GetStatements(Dialect dialect)
        {
            throw new System.NotImplementedException();
        }
    }
}