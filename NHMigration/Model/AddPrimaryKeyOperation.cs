using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping;
using NHibernate.Util;
using NHMigration.Model.Extensions;

namespace NHMigration.Model
{
    /// <summary>
    ///     Represents adding a primary key to a table.
    /// </summary>
    public class AddPrimaryKeyOperation :  IOperation
    {
        public AddPrimaryKeyOperation(){}
        public AddPrimaryKeyOperation(Table table)
        {
            Table = table;
        }

        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;
            var pk = Table.PrimaryKey;

            var start = dialect.GetIfNotExistsCreateConstraint(Table, pk.Name);
            var end = dialect.GetIfNotExistsCreateConstraintEnd(Table, pk.Name);

            var sb = new StringBuilder()
                .Append(start).AppendLine()
                .Append("alter table ")
                .Append(dialect.GetAddPrimaryKeyConstraintString(pk.Name))
                .Append('(')
                .AppendRange(Table.ColumnIterator.Select((c, i) => (i > 0 ? ", " : "") + c.GetQuotedName(dialect)))
                .Append(")")
                .AppendLine()
                .Append(end);

            return new[]
            {
                new MigrationStatement(sb.ToString())
            };

        }

        public  IOperation Inverse
        {
            get { return new DropPrimaryKeyOperation(Table); }
        }

        public Table Table { get; set; }
    }

    public class DropPrimaryKeyOperation:  IOperation
    {
        public DropPrimaryKeyOperation(){}
        public DropPrimaryKeyOperation(Table table)
        {
            Table = table;
        }

        public  IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;

            string drop = string.Format("alter table {0}{1}", Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema), dialect.GetDropPrimaryKeyConstraintString(this.Table.PrimaryKey.Name));

            return new[]
            {
                new MigrationStatement(drop),
            };
        }

        public  IOperation Inverse
        {
            get { return new AddPrimaryKeyOperation(Table); }
        }

        public Table Table { get; set; }
    }
}