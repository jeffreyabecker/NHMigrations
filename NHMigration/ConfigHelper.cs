using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace NHMigration
{
    public class ConfigHelper
    {
        #region static reflection bs
        private static readonly PropertyInfo _tableMappingsProp;
        private static readonly FieldInfo _mappingField;
        private static readonly FieldInfo _auxiliaryDatabaseObjectsField;
        private static readonly FieldInfo _classesField;
        


        static ConfigHelper()
        {
            _tableMappingsProp = typeof(Configuration).GetProperty("TableMappings", BindingFlags.Instance | BindingFlags.NonPublic);
            _mappingField = typeof(Configuration).GetField("mapping", BindingFlags.Instance | BindingFlags.NonPublic);
            _auxiliaryDatabaseObjectsField = typeof(Configuration).GetField("auxiliaryDatabaseObjects", BindingFlags.Instance | BindingFlags.NonPublic);
            //_iterateGeneratorsMethod = typeof(Configuration).GetMethod("IterateGenerators", BindingFlags.Instance | BindingFlags.NonPublic);
            
            

        }
        #endregion
        private readonly Configuration _cfg;


        public ConfigHelper(Configuration cfg)
        {
            _cfg = cfg;

            Mapping = (IMapping)_mappingField.GetValue(cfg);
            Dialect = Dialect.GetDialect(cfg.GetDerivedProperties());
            
            

        }

        public ICollection<NHibernate.Mapping.Table> Tables { get { return (ICollection<NHibernate.Mapping.Table>)_tableMappingsProp.GetValue(_cfg); } }
        public IList<IAuxiliaryDatabaseObject> AuxiliaryDatabaseObjects { get { return (IList<IAuxiliaryDatabaseObject>)_auxiliaryDatabaseObjectsField.GetValue(_cfg); } }

        public IMapping Mapping { get; protected set; }
        public Dialect Dialect { get; protected set; }

        public ICollection<PersistentClass> ClassMappings
        {
            get { return _cfg.ClassMappings; }
        }

        public ICollection<NHibernate.Mapping.Collection> CollectionMappings
        {
            get { return _cfg.CollectionMappings; }
        }
        //private IEnumerable<IPersistentIdentifierGenerator> GetGenerators(Dialect dialect)
        //{
        //    return
        //        (IEnumerable<IPersistentIdentifierGenerator>)
        //            _iterateGeneratorsMethod.Invoke(_cfg, new object[] { dialect });
        //}
        public bool ContainsTable(Table table)
        {
            return Tables.Any(t =>
                t.Catalog.IsNullOrMatches(table.Catalog)
                && t.Schema.IsNullOrMatches(table.Schema)
                && t.Name == table.Name);
        }
    }

    public static class Extensions
    {
        public static bool IsNullOrMatches(this string a, string b)
        {
            return a == null || b == null || a == b;
        }
    }
}