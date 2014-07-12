using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHMigration.Model;
using NHMigration.Operations.Extensions;
using NHMigration.Operations.Model;

namespace NHMigration.Operations
{
    /// <summary>
    ///     Represents creating a table.
    /// </summary>
    public class CreateTableOperation : IOperation
    {
        private readonly string _name;
        private readonly ICollection<Column> _columns;

        public CreateTableOperation(string name, ICollection<Column> columns)
        {
            _name = name;
            _columns = columns;

        }


        //public IEnumerable<string> GetStatements(IMigrationContext context)
        //{
        //    var dialect = context.Dialect;
        //    var defaultCatalog = context.DefaultCatalog;
        //    var defaultSchema = context.DefaultSchema;

            
        //    var sb = new StringBuilder();
    
        //    sb.Append(dialect.CreateTableString)
        //        .Append(" ")
        //        .Append(Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema))
        //        .Append(" (");

        //    var identityColumn = Table.IdentifierValue != null && Table.IdentifierValue.IsIdentityColumn(dialect) ? Table.PrimaryKey.ColumnIterator.First() : null;


        //    var commaNeeded = false;
        //    foreach (var col in Table.ColumnIterator)
        //    {
        //        if (commaNeeded)
        //        {
        //            sb.Append(", ");
        //        }
        //        sb.Append(dialect.QuoteForColumnName(col.Name)).Append(" ");

        //        if (col == identityColumn)
        //        {
        //            if (dialect.HasDataTypeInIdentityColumn)
        //            {
        //                sb.Append(dialect.GetTypeName(col.SqlTypeCode)).Append(" ");
        //            }
        //            sb.Append(dialect.GetIdentityColumnString(col.SqlTypeCode.DbType)).Append(" ");

        //        }
        //        else
        //        {

        //            if (col.HasDefaultValue())
        //            {
        //                sb.Append(" default ").Append(col.DefaultValue).Append(" ");
        //            }

        //            sb.Append(col.IsNullable ? dialect.NullColumnString : " not null");
        //        }

        //        if (col.HasCheckConstraint)
        //        {
        //            sb.Append(" check( ").Append(col.CheckConstraint).Append(") ");
        //        }
        //        commaNeeded = true;
        //    }


        //    if (!dialect.SupportsForeignKeyConstraintInAlterTable)
        //    {
        //        foreach (ForeignKey foreignKey in Table.ForeignKeyIterator)
        //        {
        //            if (foreignKey.HasPhysicalConstraint)
        //            {
        //                sb.Append(",").Append(foreignKey.SqlConstraintString(dialect, foreignKey.Name, defaultCatalog, defaultSchema));
        //            }
        //        }
        //    }
        //    sb.Append(")");

        //    sb.Append(dialect.TableTypeString);


        //    return new[]
        //    {
        //        sb.ToString()
        //    };

        //}


        public IEnumerable<string> GetStatements(Dialect dialect)
        {
            throw new System.NotImplementedException();
        }

        public void AddColumns(object columns)
        {
            
        }

        public void AddPrimaryKey(PrimaryKeyModel pkModel)
        {
            
        }
    }
}