using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping;
using NHMigration.Operations;

namespace NHMigration.Builder
{

    public abstract class MigrationBase : IMigration, IMigrationBuilder
    {
        private List<IOperation>  _operations = new List<IOperation>();
        protected MigrationBase(string version, string context)
        {
            Version = version;
            Context = context;
        }

        public IEnumerable<IOperation> GetOperations()
        {
            return _operations;
        }

        public string Version { get; private set; }

        public string Context{ get; private set; }

        public void AddOperation(IOperation operation)
        {
            _operations.Add(operation);
        }
        public ICreateTableBuilder<TColumns> CreateTable<TColumns>(string name, TColumns columns)
        {
            throw new NotImplementedException();
        }

        public void AddForeignKey(string dependentTable, string dependentColumn, string principalTable, string principalColumn = null,
            bool cascadeDelete = false, string name = null)
        {
            throw new NotImplementedException();
        }

        public void AddForeignKey(string dependentTable, string[] dependentColumns, string principalTable,
            string[] principalColumns = null, bool cascadeDelete = false, string name = null)
        {
            throw new NotImplementedException();
        }

        public void DropForeignKey(string dependentTable, string name)
        {
            throw new NotImplementedException();
        }

        public void DropForeignKey(string dependentTable, string dependentColumn, string principalTable)
        {
            throw new NotImplementedException();
        }

        public void DropForeignKey(string dependentTable, string dependentColumn, string principalTable, string principalColumn)
        {
            throw new NotImplementedException();
        }

        public void DropForeignKey(string dependentTable, string[] dependentColumns, string principalTable)
        {
            throw new NotImplementedException();
        }

        public void DropTable(string name)
        {
            throw new NotImplementedException();
        }

        public void AddColumn(string table, string name, Column column)
        {
            throw new NotImplementedException();
        }

        public void DropColumn(string table, string name)
        {
            throw new NotImplementedException();
        }

        public void AlterColumn(string table, string name, Column column)
        {
            throw new NotImplementedException();
        }

        public void AddPrimaryKey(string table, string column, string name = null, bool clustered = true)
        {
            throw new NotImplementedException();
        }

        public void AddPrimaryKey(string table, string[] columns, string name = null, bool clustered = true)
        {
            throw new NotImplementedException();
        }

        public void DropPrimaryKey(string table, string name)
        {
            throw new NotImplementedException();
        }

        public void DropPrimaryKey(string table)
        {
            throw new NotImplementedException();
        }

        public void CreateIndex(string table, string column, bool unique = false, string name = null, bool clustered = false)
        {
            throw new NotImplementedException();
        }

        public void CreateIndex(string table, string[] columns, bool unique = false, string name = null, bool clustered = false)
        {
            throw new NotImplementedException();
        }

        public void DropIndex(string table, string name)
        {
            throw new NotImplementedException();
        }

        public void DropIndex(string table, string[] columns)
        {
            throw new NotImplementedException();
        }

        public void Sql(string sql)
        {
            AddOperation(new SqlOperation{Sql = new[]{sql}});
        }
    }

}
