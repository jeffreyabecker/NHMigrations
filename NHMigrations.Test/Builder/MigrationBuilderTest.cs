using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping;
using NHMigration;
using NHMigration.Builder;
using NHMigration.Operations;
using NUnit.Framework;

namespace NHMigrations.Test.Builder
{
    public class TestBuilder : MigrationBase
    {
        public TestBuilder() : base("a", "a")
        {
            

        }
    }
    [TestFixture]
    public class MigrationBuilderFixture
    {

        public void CanCreateATable()
        {

        }
    }

    
}
