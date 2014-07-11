using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.SqlTypes;

namespace NHMigration.Operations
{
    public class CreateTableGeneratorOperation : IOperation
    {
        private readonly string _tableName;
        private readonly string _columnName;
        private readonly SqlType _columnType;

        public CreateTableGeneratorOperation(string tableName, string columnName, SqlType columnType)
        {
            _tableName = tableName;
            _columnName = columnName;
            _columnType = columnType;
        }

        public IEnumerable<string> GetStatements(IMigrationContext context)
        {
            var sb = new StringBuilder();
            var dialect = context.Dialect;
            var defaultSchema = context.DefaultSchema;
            var defaultCatalog = context.DefaultCatalog;

            var fmt1 = "{0} {2} ({3} {4})";
            var fmt2 = "insert into {0} (1)";
            var quotedTable = dialect.QuoteForTableName(_tableName);
            var quotedColumn = dialect.QuoteForColumnName(_columnName);
            var columnTypeName = dialect.GetTypeName(_columnType);
            return new[]
            {
                String.Format(fmt1, dialect.CreateTableString, quotedTable, quotedColumn, columnTypeName),
                string.Format(fmt2, quotedTable)
            };
        }
    }
}