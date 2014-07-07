using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Mapping;

namespace NHMigration.Model
{
    public class AddForeignKeyOperation : IOperation
    {
        public AddForeignKeyOperation(){}

        public AddForeignKeyOperation(ForeignKey foreignKey)
        {
            ForeignKey = foreignKey;
        }
        public NHibernate.Mapping.ForeignKey ForeignKey { get; set; }

        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;
            var colspan = ForeignKey.ColumnSpan;

            var primaryKey = (ForeignKey.IsReferenceToPrimaryKey
                ? ForeignKey.ReferencedTable.PrimaryKey.ColumnIterator
                : ForeignKey.ReferencedColumns).Select(c=>c.GetQuotedName(dialect)).Take(colspan).ToArray();
            var foreignKey = ForeignKey.ColumnIterator.Select(c => c.GetQuotedName(dialect)).Take(colspan).ToArray();

            var tableName = ForeignKey.ReferencedTable.GetQualifiedName(dialect, defaultCatalog, defaultSchema);

            var constraint = dialect.GetAddForeignKeyConstraintString(ForeignKey.Name, foreignKey, tableName, primaryKey,
                ForeignKey.IsReferenceToPrimaryKey);

            if (dialect.SupportsCascadeDelete && ForeignKey.CascadeDeleteEnabled)
            {
                constraint = constraint + " on delete cascade";
            }

            return new[] {new MigrationStatement(constraint),};


        }

        public IOperation Inverse
        {
            get { return new DropForeignKeyOperation(ForeignKey); }
        }
    }

    public class DropForeignKeyOperation : IOperation
    {


        public DropForeignKeyOperation(){}
        public DropForeignKeyOperation(ForeignKey foreignKey)
        {
            ForeignKey = foreignKey;
        }

        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            throw new NotImplementedException();
        }

        public IOperation Inverse { get{return new AddForeignKeyOperation(ForeignKey);} }

        public ForeignKey ForeignKey { get; set; }
    }
}