using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Exceptions;

namespace NHMigration.Model
{
    public interface IMigrationStatement
    {
        string Sql { get; }
        string BatchTerminator { get; }
    }

    public class MigrationStatementResult : IEnumerable<IMigrationStatement>
    {
        private readonly string[] _statements;

        public MigrationStatementResult(params string[] statements)
        {
            _statements = statements;
        }

        public MigrationStatementResult(params StringBuilder[] sbs)
        {
            _statements = sbs.Select(s => s.ToString()).ToArray();
        }

        public IEnumerator<IMigrationStatement> GetEnumerator()
        {
            return _statements.Select(s => (IMigrationStatement)new MigrationStatement(s)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class MigrationStatement : IMigrationStatement
    {

        public MigrationStatement(){}

        public MigrationStatement(String sql)
        {
            Sql = sql;
        }


        public string Sql { get; set; }

        public string BatchTerminator { get; set; }
    }

    public interface IOperation
    {
        IEnumerable<IMigrationStatement> GetStatements(IMigrationContext context);
        IOperation Inverse { get; }
    }
}
