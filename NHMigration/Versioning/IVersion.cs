using System;
using NHibernate.Cfg;

namespace NHMigration.Versioning
{
    public interface IVersion : IComparable<IVersion>
    {
        string MigrationContextName { get; }
        string Version { get; set; }
        Configuration Configuration { get; set; }
    }
}