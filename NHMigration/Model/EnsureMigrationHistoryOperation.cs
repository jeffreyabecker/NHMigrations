using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Mapping;
using NHibernate.SqlTypes;
using NHMigration.Versioning;

namespace NHMigration.Model
{
    public class EnsureMigrationHistoryOperation : IOperation
    {
        private readonly IVersioningStrategy _versioningStrategy;

        public EnsureMigrationHistoryOperation(IVersioningStrategy versioningStrategy)
        {
            _versioningStrategy = versioningStrategy;
        }
        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            return _versioningStrategy.EnsureDbObjectsStatements;
        }
    }

    public class UpdateMigrationHistoryToVersion : IOperation
    {
        private readonly IVersioningStrategy _versioningStrategy;
        private readonly string _version;

        public UpdateMigrationHistoryToVersion(IVersioningStrategy versioningStrategy, string version)
        {
            _versioningStrategy = versioningStrategy;
            _version = version;
        }

        public IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context)
        {
            return _versioningStrategy.UpdateVersionToStatements(_version);
        }
    }
}
