using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHMigration.Model
{
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
}