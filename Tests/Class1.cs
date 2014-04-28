using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping.ByCode;
using NHibernate.Persister.Entity;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void CanCat()
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.ConnectionString = "Data Source=d:\\databases\\foo.db3";
            });
            var mapper = new ConventionModelMapper();
            mapper.BeforeMapClass += mapper_BeforeMapClass;
            cfg.AddMapping(mapper.CompileMappingFor(new[]{typeof(Cat)}));
            var bf = new BinaryFormatter();
            Configuration old = null;
            using (var fs = File.OpenRead(@"d:\cat.cfg"))
            {
                old = (Configuration) bf.Deserialize(fs);
            }
            cfg.BuildMappings();
            old.BuildMappings();

        }

        void mapper_BeforeMapClass(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
            classCustomizer.Id(id=> id.Generator(Generators.GuidComb));
        }
    }

    public class Cat
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        //public virtual long CountOfHairs { get; set; }
    }
}
