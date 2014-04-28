﻿using NHMigration.Versioning;

namespace NHMigration
{
    public interface IMigration
    {
        string MigrationContext { get; }
        IVersion Version { get; }
        IVersion Execute(MigrationDatabaseContex ctx);
    }
}