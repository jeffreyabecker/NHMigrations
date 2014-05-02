using System.Data;
using NHibernate.Linq;
using NHibernate.Mapping;

namespace NHMigration.Operations
{

    public class DropForeignKeyOperation : IOperation
    {
        public Table Table { get; set; }
        public string Name { get; set; }

        public DropForeignKeyOperation(ForeignKey fk)
        {
            Table = fk.ReferencedTable;
            Name = fk.Name;
        }

        public void Apply(MigrationDatabaseContex ctx)
        {
           
            var beginifExists = ctx.Dialect.GetIfExistsDropConstraint(Table, Name);
            string drop = string.Format("alter table {0} {1}", Table.GetQualifiedName(ctx.Dialect, ctx.DefaultCatalog, ctx.DefaultSchema),
																	ctx.Dialect.GetDropForeignKeyConstraintString(Name));
            var endIfExists = ctx.Dialect.GetIfExistsDropConstraintEnd(Table, Name);

            var sql = beginifExists + System.Environment.NewLine + drop + System.Environment.NewLine + endIfExists;
            var cmd = ctx.GenerateCommand(sql);
            ctx.Log(cmd);
            if (ctx.Mode == MigrationMode.ScriptAndExecute)
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}