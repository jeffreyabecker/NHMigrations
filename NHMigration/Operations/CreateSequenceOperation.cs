using System;
using System.Collections.Generic;

namespace NHMigration.Operations
{
    public class CreateSequenceOperation : IOperation
    {
        private readonly string _name;
        private readonly string _catalog;
        private readonly string _schema;
        private readonly string _parameters;

        public CreateSequenceOperation(string name, string catalog, string schema, string parameters)
        {
            _name = name;
            _catalog = catalog;
            _schema = schema;
            _parameters = parameters;
        }

        public IEnumerable<string> GetStatements(IMigrationContext context)
        {
            var dialect = context.Dialect;
            var defaultSchema = context.DefaultSchema;
            var defaultCatalog = context.DefaultCatalog;
            var sequenceName = dialect.Qualify(_catalog, _schema, _name);

            var ddl = dialect.GetCreateSequenceString(sequenceName);
            if (!String.IsNullOrWhiteSpace(_parameters))
            {
                ddl = ddl + " " + _parameters;
            }
            return new[] {ddl};
        }
    }
}
