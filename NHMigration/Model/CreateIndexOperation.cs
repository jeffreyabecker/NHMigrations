using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Dialect;
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

    /// <summary>
    ///     Represents creating a database UniqueKey.
    /// </summary>
    public class CreateUniqueKeyOperation : IOperation
    {
        private readonly UniqueKey _uniqueKey;


        public CreateUniqueKeyOperation(UniqueKey uniqueKey)
        {
            _uniqueKey = uniqueKey;
            
        }


        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            var create = _uniqueKey.SqlCreateString(context.Dialect, null, context.DefaultCatalog, context.DefaultSchema);
            return new MigrationStatementResult(create);

        }

        public IOperation Inverse
        {
            get { return new DropUniqueKeyOperation(_uniqueKey); }
        }
    }

    /// <summary>
    ///     Represents creating a database UniqueKey.
    /// </summary>
    public class DropUniqueKeyOperation : IOperation
    {
        private readonly UniqueKey _uniqueKey;

        public DropUniqueKeyOperation(UniqueKey uniqueKey)
        {
            _uniqueKey = uniqueKey;
        }


        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext ctx)
        {
            var drop = _uniqueKey.SqlDropString(ctx.Dialect, ctx.DefaultCatalog, ctx.DefaultSchema);
            return new MigrationStatementResult(drop);
        }

        public IOperation Inverse
        {
            get { return new CreateUniqueKeyOperation(_uniqueKey); }
        }
    }
}