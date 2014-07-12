using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Mapping;
using NHMigration.Errors;
using NHMigration.Operations;
using NHMigration.Operations.Model;

namespace NHMigration.Builder
{
    public interface ICreateTableBuilder<TColumns>
    {

        ICreateTableBuilder<TColumns> PrimaryKey(Expression<Func<TColumns, object>> keyExpression, bool identity = false, string name = null,
            bool clustered = true);

        ICreateTableBuilder<TColumns> Index(Expression<Func<TColumns, object>> indexExpression, string name = null,
            bool unique = false, bool clustered = false);

        ICreateTableBuilder<TColumns> ForeignKey(string principalTable,
            Expression<Func<TColumns, object>> dependentKeyExpression, bool cascadeDelete = false, string name = null);

    }

    public class CreateTableBuilder<TColumns> : ICreateTableBuilder<TColumns>
    {
        private readonly TColumns _columns;
        private CreateTableOperation _operation;
        private List<Column> _columnsList;
        public CreateTableBuilder(string name, TColumns columns)
        {
            _columns = columns;
            _columnsList = ExtractColumnList(columns);
            _operation = new CreateTableOperation(name, _columnsList);
            
        }

        private List<Column> ExtractColumnList(object columns)
        {
            return  columns.GetType().GetProperties()
                .Where(p => p.CanRead && p.PropertyType == typeof (Column))
                .Select(p => new {Name = p.Name, Col = (Column) p.GetValue(columns)})
                .Select(c =>
                {
                    c.Col.Name = String.IsNullOrEmpty(c.Col.Name) ? c.Name : c.Col.Name;
                    return c.Col;
                }).ToList();

        }

        public ICreateTableBuilder<TColumns> PrimaryKey(Expression<Func<TColumns, object>> keyExpression, bool identity = false, string name = null,
            bool clustered = true)
        {
            var pkCols = ExtractColumnList(keyExpression.Compile()(_columns));
            if (identity == true && pkCols.Count > 1)
            {
                throw new TooManyColumnsForIdentityException(pkCols);
            }
            var notInTable = pkCols.Where(pkc => !_columnsList.Contains(pkc)).ToList();
            if (notInTable.Any())
            {
                throw new PKContainsColumnsNotInTableException(notInTable);
            }
            var pkModel = new PrimaryKeyModel(pkCols, identity, name, clustered);
            _operation.AddPrimaryKey(pkModel);
        }

        public ICreateTableBuilder<TColumns> Index(Expression<Func<TColumns, object>> indexExpression, string name = null, bool unique = false, bool clustered = false)
        {
            throw new NotImplementedException();
        }

        public ICreateTableBuilder<TColumns> ForeignKey(string principalTable, Expression<Func<TColumns, object>> dependentKeyExpression, bool cascadeDelete = false,
            string name = null)
        {
            throw new NotImplementedException();
        }

        public CreateTableOperation Operation
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}