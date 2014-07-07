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

        public AddPrimaryKeyOperation(Table table)
        {
            _table = table;
        }

        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;
            var pk = _table.PrimaryKey;

            var start = dialect.GetIfNotExistsCreateConstraint(_table, pk.Name);
            var end = dialect.GetIfNotExistsCreateConstraintEnd(_table, pk.Name);

            var sb = new StringBuilder()
                .Append(start).AppendLine()
                .Append("alter table ")
                .Append(dialect.GetAddPrimaryKeyConstraintString(pk.Name))
                .Append('(')
                .AppendRange(_table.ColumnIterator.Select((c, i) => (i > 0 ? ", " : "") + c.GetQuotedName(dialect)))
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
            get { return new DropPrimaryKeyOperation(_table); }
        }

        private Table _table;

    
    }

    public class DropPrimaryKeyOperation:  IOperation
    {

        public DropPrimaryKeyOperation(Table table)
        {
            _table = table;
        }

        public  IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;

            string drop = string.Format("alter table {0}{1}", _table.GetQualifiedName(dialect, defaultCatalog, defaultSchema), dialect.GetDropPrimaryKeyConstraintString(this._table.PrimaryKey.Name));

            return new[]
            {
                new MigrationStatement(drop),
            };
        }

        public  IOperation Inverse
        {
            get { return new AddPrimaryKeyOperation(_table); }
        }

        private readonly Table _table;

        
    }
}