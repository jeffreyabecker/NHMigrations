using System;
using NHibernate.Cfg;
using NHMigration.Util;

namespace NHMigration.Versioning
{
    public class MigrationHistory : IVersion
    {
        public virtual string Version { get; set; }
        public virtual string MigrationContextName { get; set; }
        public virtual Configuration Configuration { get; set; }

        public int CompareTo(IVersion that)
        {
            return String.CompareOrdinal(this.Version, that.Version);
        }

        public override bool Equals(object obj)
        {
            var that = obj as MigrationHistory;
            if (that == null) return false;
            return this.Version == that.Version && this.MigrationContextName == that.MigrationContextName;
        }

        public override int GetHashCode()
        {
            return (int)MurmurHash.Hash(Version + MigrationContextName);
        }
    }
}