using System;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode.Impl;
using NHMigration.Model;

namespace NHMigration.Versioning
{
    public interface IVersioningStrategy
    {
        IEnumerable<string> EnsureDbObjectsStatements { get; }
        IEnumerable<string> GetUpdateVersionStatements(string version, string migrationContext);
    }

  
}