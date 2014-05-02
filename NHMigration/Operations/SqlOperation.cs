using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping;
using NHibernate.SqlCommand;

namespace NHMigration.Operations
{
    public class SqlOperation : IOperation
    {
        public static SqlOperation GetDropSqlOperationForAuxObj(IAuxiliaryDatabaseObject auxDbObj)
        {
            var auxobj = auxDbObj as AbstractAuxiliaryDatabaseObject;
            var res = new SqlOperation
            {
                Dialects = auxobj.DialectScopes.ToList(),
                Parameters = auxobj.Parameters,
                Sql = auxobj.SqlDropString(null,string.Empty, string.Empty)
            };
            return res;
        }
        public static SqlOperation GetCreateSqlOperationForAuxObj(IAuxiliaryDatabaseObject auxDbObj)
        {
            var auxobj = auxDbObj as AbstractAuxiliaryDatabaseObject;
            var res = new SqlOperation
            {
                Dialects = auxobj.DialectScopes.ToList(),
                Parameters = auxobj.Parameters,
                Sql = auxobj.SqlCreateString(null, null, string.Empty, string.Empty)
            };
            return res;
        }

        public ICollection<string> Dialects { get; set; }
        public IDictionary<string,string> Parameters { get; set; }
        public string Sql { get; set; }
       

        public void Apply(MigrationDatabaseContex ctx)
        {
            if (Dialects.Count == 0 || Dialects.Contains(ctx.Dialect.GetType().FullName))
            {
                //TODO: Figure out how parameters work and stuff;
                var cmd = ctx.GenerateCommand(new SqlString(Sql));
                ctx.Log(cmd);
                if (ctx.Mode == MigrationMode.ScriptAndExecute)
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}