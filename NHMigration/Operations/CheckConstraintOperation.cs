using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping;

namespace NHMigration.Model
{




    //public class CreateCheckConstraintOperation : IOperation
    //{
    //    public Table Table { get; set; }

    //    public Column Column { get; set; }

    //    public CreateCheckConstraintOperation(Table table)
    //    {
    //        Table = table;
    //    }

    //    public CreateCheckConstraintOperation(Column column)
    //    {
    //        Column = column;
    //    }

    //    public IEnumerable<string> GetStatements(IMigrationContext context)
    //    {
    //        var dialect = context.Dialect;
    //        var defaultCatalog = context.DefaultCatalog;
    //        var defaultSchema = context.DefaultSchema;
    //        if (Table != null && dialect.SupportsTableCheck)
    //        {
    //            int i = 1;
    //            foreach (var check in Table.CheckConstraintsIterator)
    //            {
    //                var sb = new StringBuilder();
    //                sb.Append("alter table add constraint ")
    //                    .Append(dialect.QuoteForTableName("CK_" + Table.Name + "_" + i))
    //                    .Append(" check (").Append(check).Append(")");
    //                yield return new MigrationStatement(sb.ToString());
    //                i++;
    //            }
    //        }
    //        if (Column != null)
    //        {
    //            var sb = new StringBuilder();
    //            sb.Append("alter table add constraint ")
    //                .Append(dialect.QuoteForTableName("CK_" + Table.Name + "_" + Column.Name))
    //                .Append(" check (").Append(Column.CheckConstraint).Append(")");
    //            yield return new MigrationStatement(sb.ToString());
    //        }
    //    }


    //}

    //public class DropCheckConstraintOperation : IOperation
    //{
    //    public Column Column { get; set; }
    //    public Table Table { get; set; }

    //    public DropCheckConstraintOperation(Table table)
    //    {
    //        Table = table;
    //    }

    //    public DropCheckConstraintOperation(Column column)
    //    {
    //        Column = column;
    //    }

    //    public IEnumerable<string> GetStatements(IMigrationContext context)
    //    {
    //      throw new NotImplementedException();
    //    }


    //}


}