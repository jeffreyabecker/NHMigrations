using System;
using NHibernate;
using NHibernate.Engine;
using NHMigration.Versioning;

namespace NHMigration
{
    public class Migrator
    {
        private readonly IVersioningStrategy _versioningStrategy;
        private readonly ISessionFactoryImplementor _sessionFactory;
        public Migrator(ISessionFactory sessionFactory, IVersioningStrategy versioningStrategy)
        {
            _versioningStrategy = versioningStrategy;
            _sessionFactory = (ISessionFactoryImplementor) sessionFactory;
        }

        public void Execute(string targetVersion, MigrationMode mode = MigrationMode.ScriptAndExecute, string migrationContext = null, Action<string> log = null)
        {
            using (var ctx = new MigrationDatabaseContex(_sessionFactory, mode, migrationContext, log))
            {
                _versioningStrategy.Initialize(ctx);
                var current = _versioningStrategy.GetCurrentVersion(ctx);

                var migrations = _versioningStrategy.GetMigrationsToExecute(current, targetVersion, ctx);
                IVersion lastVersion = null;
                foreach (var m in migrations)
                {
                    lastVersion = m.Execute(ctx);
                }
                if (lastVersion != null)
                {
                    _versioningStrategy.SetCurrentVersion(lastVersion, ctx);
                }
            }
        }


    }

    
    
}
