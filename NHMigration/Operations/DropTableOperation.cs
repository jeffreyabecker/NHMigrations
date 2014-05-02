using System;
using NHibernate.Mapping;

namespace NHMigration.Operations
{
    public class DropTableOperation : IOperation
    {
        public Table Table { get; set; }
        public DropTableOperation (){}
        public DropTableOperation(Table table)
        {
            this.Table = table;
        }

        public void Apply(MigrationDatabaseContex ctx)
        {

            var drop = Table.SqlDropString(ctx.Dialect, ctx.DefaultCatalog, ctx.DefaultSchema);

            var cmd = ctx.GenerateCommand(drop);
            ctx.Log(cmd);
            if (ctx.Mode == MigrationMode.ScriptAndExecute)
            {
                cmd.ExecuteNonQuery();
            }

        }
    }
}