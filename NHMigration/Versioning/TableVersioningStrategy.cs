
using NHibernate.Type;

namespace NHMigration.Versioning
{
    public class MigrationHistoryMapping : NHibernate.Mapping.ByCode.Conformist.ClassMapping<MigrationHistory>
    {
        public MigrationHistoryMapping()
        {
            Table("__NHibernateMigrationHistory");
            ComposedId(id =>
            {
                id.Property(h=>h.Version,p=>p.Length(300));
                id.Property(h=>h.MigrationContextName,p=>p.Length(300));
            });
            Property(o=>o.Configuration, m =>
            {
                m.Column(cm => cm.SqlType("varbinary(MAX)"));
                m.Type<SerializableType>();
            });
        }
    }


}