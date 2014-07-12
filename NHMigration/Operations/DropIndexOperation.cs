using System.Collections.Generic;
using NHibernate.Mapping;
using NHibernate.Util;

namespace NHMigration.Operations
{
    /// <summary>
    ///     Represents creating a database index.
    /// </summary>
    public class DropIndexOperation : IOperation
    {
        //private readonly UniqueKey _uk;
        //private readonly Index _index;
        //public DropIndexOperation(UniqueKey uk)
        //{
        //    _uk = uk;
        //}

        //public DropIndexOperation(Index index)
        //{
        //    _index = index;
        //}

        
        //public IEnumerable<string> GetStatements(IMigrationContext context)
        //{
        //    var dialect = context.Dialect;
        //    var defaultCatalog = context.DefaultCatalog;
        //    var defaultSchema = context.DefaultSchema;
        //    if (_index != null)
        //    {
        //        string drop = string.Format("drop index {0}",
        //            StringHelper.Qualify(_index.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema),
        //                _index.Name));
        //        return new[] {drop};
        //    }
        //    else
        //    {
        //        string drop = string.Format("drop index {0}",
        //            StringHelper.Qualify(_uk.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema),
        //                _uk.Name));
        //        return new[] { drop };
        //    }
        //}

        public IEnumerable<string> GetStatements(Dialect dialect)
        {
            throw new System.NotImplementedException();
        }
    }
}