using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping;

namespace NHMigration.Errors
{
    public class MigrationException : Exception
    {
        protected MigrationException(Exception inner, String format, params object[] formatArgs)
            : base(String.Format(format, formatArgs), inner)
        {
            
        }
        protected MigrationException( String format, params object[] formatArgs)
            : base(String.Format(format, formatArgs))
        {

        }
    }

    public class TooManyColumnsForIdentityException : MigrationException
    {
        public List<Column> PkColumns { get; private set; }

        public TooManyColumnsForIdentityException(List<Column> pkColumns)
            :base("More than one column was specified for an identity primary key")
        {
            PkColumns = pkColumns;
        }
    }
    public class PKContainsColumnsNotInTableException : MigrationException
    {
        public List<Column> PkColumns { get; private set; }
        

        public PKContainsColumnsNotInTableException(List<Column> pkColumns)
            : base("The primary key specified contains columns not contained in the table defintion")
        {
            PkColumns = pkColumns;
        }
    }
    
}
