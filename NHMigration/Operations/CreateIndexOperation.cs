using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping;
using NHibernate.Util;
using NHMigration.Operations.Extensions;

namespace NHMigration.Operations
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
        public IEnumerable<string> GetStatements(IMigrationContext context)
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

            return new []{sb.ToString()};

        }

    }

    ///// <summary>
    /////     Represents creating a database UniqueKey.
    ///// </summary>
    //public class CreateUniqueKeyOperation : IOperation
    //{
    //    private readonly UniqueKey _uniqueKey;


    //    public CreateUniqueKeyOperation(UniqueKey uniqueKey)
    //    {
    //        _uniqueKey = uniqueKey;
            
    //    }


    //    public IEnumerable<string> GetStatements(IMigrationContext context)
    //    {
    //        var create = _uniqueKey.SqlCreateString(context.Dialect, null, context.DefaultCatalog, context.DefaultSchema);
    //        return new[] { create };

    //    }

    //}

    ///// <summary>
    /////     Represents creating a database UniqueKey.
    ///// </summary>
    //public class DropUniqueKeyOperation : IOperation
    //{
    //    private readonly UniqueKey _uniqueKey;

    //    public DropUniqueKeyOperation(UniqueKey uniqueKey)
    //    {
    //        _uniqueKey = uniqueKey;
    //    }


    //    public IEnumerable<string> GetStatements(IMigrationContext ctx)
    //    {
    //        var drop = _uniqueKey.SqlDropString(ctx.Dialect, ctx.DefaultCatalog, ctx.DefaultSchema);
    //        return new[] { drop };
    //    }


    //}
}