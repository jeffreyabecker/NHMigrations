using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping;

namespace NHMigration.Model
{
    public class AddForeignKeyOperation : IOperation
    {
        
        public AddForeignKeyOperation(ForeignKey foreignKey)
        {
            _foreignKey = foreignKey;
        }

        private readonly NHibernate.Mapping.ForeignKey _foreignKey;

        public IEnumerable<string> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;

            if (!dialect.SupportsForeignKeyConstraintInAlterTable)
            {
                throw new NotSupportedException(String.Format("{0} does not support foreign key in alter table", dialect.GetType().Name));
            }

            var colspan = _foreignKey.ColumnSpan;

            var primaryKey = (_foreignKey.IsReferenceToPrimaryKey
                ? _foreignKey.ReferencedTable.PrimaryKey.ColumnIterator
                : _foreignKey.ReferencedColumns).Select(c=>c.GetQuotedName(dialect)).Take(colspan).ToArray();
            var foreignKey = _foreignKey.ColumnIterator.Select(c => c.GetQuotedName(dialect)).Take(colspan).ToArray();

            var tableName = _foreignKey.ReferencedTable.GetQualifiedName(dialect, defaultCatalog, defaultSchema);

            var constraint = dialect.GetAddForeignKeyConstraintString(_foreignKey.Name, foreignKey, tableName, primaryKey,
                _foreignKey.IsReferenceToPrimaryKey);

            if (dialect.SupportsCascadeDelete && _foreignKey.CascadeDeleteEnabled)
            {
                constraint = constraint + " on delete cascade";
            }

            return new[] { constraint};


        }


    }

    public class DropForeignKeyOperation : IOperation
    {
        private readonly ForeignKey _foreignKey;
        public DropForeignKeyOperation(ForeignKey foreignKey)
        {
            _foreignKey = foreignKey;
        }

        public IEnumerable<string> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;


            if (!dialect.SupportsForeignKeyConstraintInAlterTable)
            {
                throw new NotSupportedException(String.Format("{0} does not support foreign key in alter table", dialect.GetType().Name));
            }
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;

            var tableName = _foreignKey.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema);
            var fkDrop = dialect.GetDropForeignKeyConstraintString(_foreignKey.Name);

            string drop = string.Format("alter table {0} {1}", tableName, fkDrop);
            return new[]
            {
                drop,
            };

        }


        
    }
}