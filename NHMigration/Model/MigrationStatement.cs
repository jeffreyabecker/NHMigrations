using System;

namespace NHMigration.Model
{
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
}