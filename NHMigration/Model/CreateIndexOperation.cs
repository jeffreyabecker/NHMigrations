using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Mapping;
using NHibernate.Util;
using NHMigration.Model.Extensions;

namespace NHMigration.Model
{
    /// <summary>
    ///     Represents creating a database index.
    /// </summary>
    public class CreateIndexOperation : IOperation
    {
        public CreateIndexOperation(){}

        public CreateIndexOperation(Index index)
        {
            Index = index;
        }

        public Index Index { get; set; }
        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;

            var sb = new StringBuilder("create index ")
                            .Append(dialect.QualifyIndexName ? Index.Name : StringHelper.Unqualify(Index.Name))
                            .Append(" on ")
                            .Append(Index.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema))
                            .Append(" (");

            sb.AppendRange(Index.ColumnIterator.Select((c, i) => (i > 0 ? ", " : "") + c.GetQuotedName(dialect)));
            sb.Append(")");

            return new MigrationStatementResult(sb);

        }

        public IOperation Inverse
        {
            get { return new DropIndexOperation(Index); }
        }
    }

    /// <summary>
    ///     Represents creating a database index.
    /// </summary>
    public class DropIndexOperation : IOperation
    {
        public DropIndexOperation() { }

        public DropIndexOperation(Index index)
        {
            Index = index;
        }

        public Index Index { get; set; }
        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultCatalog = context.DefaultCatalog;
            var defaultSchema = context.DefaultSchema;
            string drop = string.Format("drop index {0}", StringHelper.Qualify(Index.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema), Index.Name));
            return new MigrationStatementResult(drop);
        }

        public IOperation Inverse
        {
            get { return new CreateIndexOperation(Index); }
        }
    }
}