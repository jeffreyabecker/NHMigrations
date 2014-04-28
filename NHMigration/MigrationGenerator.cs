using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NHMigration.Operations;
using NHMigration.Versioning;

namespace NHMigration
{
    public class MigrationGenerator
    {

        public class ConfigHelper
        {
            private readonly Configuration _c;
            private static PropertyInfo _tableMappings;

            static ConfigHelper()
            {
                _tableMappings = typeof (Configuration).GetProperty("TableMappings",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            }
            public ConfigHelper(Configuration c)
            {
                _c = c;
                c.BuildMappings();
            }

            public ICollection<Table> Tables
            {
                get { return (ICollection<Table>) _tableMappings.GetValue(_c); }
            }
        }

        public class TablePair
        {
            public Table Prev { get; set; }
            public Table Curent { get; set; }
        }
        public IEnumerable<IOperation> GenerateOperations(IVersion prevVersion, IVersion currentVersion)
        {
            if (prevVersion == null)
            {
                foreach (var operation in GetCreateOperations(currentVersion))
                {
                    yield return operation;
                }

            }
            else
            {
                var previousConfig = new ConfigHelper(prevVersion.Configuration);
                var currentConfig = new ConfigHelper(currentVersion.Configuration);

                var added = GetAddedTables(previousConfig, currentConfig);
                foreach (var )

            }
        }

        private IEnumerable<IOperation> GetCreateOperations(IVersion currentVersion)
        {
            var cfg = new ConfigHelper(currentVersion.Configuration);


        }

        private IEnumerable<Table> GetAddedTables(ConfigHelper previousConfig, ConfigHelper currentConfig)
        {
            throw new NotImplementedException();
        }
    }
}
