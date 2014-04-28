using System;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode.Impl;

namespace NHMigration.Versioning
{
    public interface IVersioningStrategy
    {
        void Initialize(MigrationDatabaseContex ctx);
        IVersion GetCurrentVersion(MigrationDatabaseContex ctx);
        void SetCurrentVersion(IVersion version, MigrationDatabaseContex ctx);
        


        IEnumerable<IMigration> GetMigrationsToExecute(IVersion current, string targetVersion, MigrationDatabaseContex ctx);
    }

    public class DbVersion : IVersion
    {
        public string MigrationContextName { get;  set; }
        public string Version { get; set; }
        public Configuration Configuration { get; set; }

        public DbVersion(){}
        public int CompareTo(IVersion other)
        {
            return String.CompareOrdinal(this.Version, other.Version);
        }
        
    }
}