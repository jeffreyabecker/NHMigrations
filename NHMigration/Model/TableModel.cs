using NHibernate.Dialect;

namespace NHMigration.Model
{
    public class TableModel
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Catalog { get; set; }

        public static implicit operator TableModel(string tableName)
        {
            return new TableModel {Name = tableName};
        }

        public virtual string GetQualifiedName(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            string quotedName = dialect.QuoteForTableName(Name);
            string usedSchema = Schema == null ? defaultSchema : dialect.QuoteForSchemaName(Schema);
            string usedCatalog = Catalog ?? defaultCatalog;
            return dialect.Qualify(usedCatalog, usedSchema, quotedName);
        }
    }
}