using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate;
using NHibernate.AdoNet.Util;
using NHibernate.Cfg;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;

namespace NHMigration.Versioning
{
    //Server=(localdb)\v11.0;Integrated Security=true;AttachDbFileName=C:\MyFolder\MyData.mdf;
    public class TableVersioningStrategy : IVersioningStrategy
    {

        public const string TableName = "__NHMigrationHistory";
        public const string VersionColumn = "Version";
        public const string MigrationContextColumn = "MigrationContext";
        public const string ConfigurationColumn = "Configuration";
        public const int NameColumnLength = 300;
        public const int BlobLen = 1024*1024*50;

        private bool _tableExists = false;

        public void Initialize(MigrationDatabaseContex ctx)
        {
            
            var schema = ctx.Dialect.GetDataBaseSchema(ctx.Connection as DbConnection);
            var tables = schema.GetTables(null, null, TableName, new[] {"TABLE"});
            if (tables.Rows.Count == 0)
            {
                var version = ctx.Dialect.QuoteForColumnName(VersionColumn);
                var migrationContext = ctx.Dialect.QuoteForColumnName(MigrationContextColumn);
                var configuration = ctx.Dialect.QuoteForColumnName(ConfigurationColumn);
                var table = ctx.Dialect.QuoteForTableName(TableName);
                var nameType = ctx.Dialect.GetTypeName(SqlTypeFactory.GetAnsiString(NameColumnLength));
                var blobType = ctx.Dialect.GetTypeName(SqlTypeFactory.GetBinaryBlob(BlobLen));

                var create = new SqlString(ctx.Dialect.CreateTableString, " ", table, StringHelper.OpenParen,
                    version, " ", nameType, StringHelper.CommaSpace,
                    migrationContext, " ", nameType, StringHelper.CommaSpace,
                    configuration, " ", blobType);

                var cmd = ctx.GenerateCommand(create);
                ctx.Log(cmd, FormatStyle.Ddl);
                if (ctx.Mode == MigrationMode.ScriptAndExecute)
                {
                    cmd.ExecuteNonQuery();
                    _tableExists = true;
                }
            }
            else
            {
                _tableExists = true;
            }

        }

        public IVersion GetCurrentVersion(MigrationDatabaseContex ctx)
        {
            var alias = "vtb";
            var version =  ctx.Dialect.QuoteForColumnName(VersionColumn);
            var migrationContext = ctx.Dialect.QuoteForColumnName(MigrationContextColumn);
            var configuration =  ctx.Dialect.QuoteForColumnName(ConfigurationColumn);
            var table = ctx.Dialect.QuoteForTableName(TableName);

            var b = ctx.CreateSqlBuilder<SqlSelectBuilder>();

            b.SetSelectClause(String.Join(StringHelper.CommaSpace, version, migrationContext, configuration));
            b.SetFromClause(table, alias);
            b.SetOrderByClause(new SqlString(version, " DESC"));
            b.SetWhereClause(alias, new[] {migrationContext}, NHibernateUtil.AnsiString);

            var query = ctx.Dialect.GetLimitString(b.ToSqlString(), null, new SqlString("1"));

            var cmd = ctx.GenerateCommand(b.ToSqlString(), SqlTypeFactory.GetAnsiString(NameColumnLength));
            
            using (cmd)
            {
                ((IDataParameter) cmd.Parameters[0]).Value = ctx.Name;
                
                if (ctx.Mode == MigrationMode.ScriptAndExecute && _tableExists)
                {
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var result = new DbVersion();
                        result.Version = reader.GetString(0);
                        result.MigrationContextName = reader.GetString(1);
                        var bytes = (byte[]) reader.GetValue(2);
                        result.Configuration = DeserializeConfiguration(bytes);
                        return result;
                    }
                }
            }
            return null;
        }

        public void SetCurrentVersion(IVersion version, MigrationDatabaseContex ctx)
        {
            var alias = "vtb";
            var vcol = ctx.Dialect.QuoteForColumnName(VersionColumn);
            var migrationContext = ctx.Dialect.QuoteForColumnName(MigrationContextColumn);
            var configuration = ctx.Dialect.QuoteForColumnName(ConfigurationColumn);
            var table = ctx.Dialect.QuoteForTableName(TableName);

            var b = ctx.CreateSqlBuilder<SqlInsertBuilder>();
            b.SetTableName(table);
            b.AddColumn(vcol, version.Version, (ILiteralType)NHibernateUtil.AnsiString);
            b.AddColumn(migrationContext, version.MigrationContextName, (ILiteralType)NHibernateUtil.AnsiString);
            b.AddColumn(configuration, SerializeConfiguration(version.Configuration),
                (ILiteralType) NHibernateUtil.BinaryBlob);
            var cmd = ctx.GenerateCommand(b.ToSqlCommandInfo());
            ctx.Log(cmd);
            if (ctx.Mode == MigrationMode.ScriptAndExecute && _tableExists)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<IMigration> GetMigrationsToExecute(IVersion current, string targetVersion, MigrationDatabaseContex ctx)
        {
            var mappedAssemblies = ctx.GetAllClassMetatdata()
                .Select(m => m.Value)
                .Cast<AbstractEntityPersister>()
                .Where(c => c != null)
                .Select(c => c.EntityType.ReturnedClass.Assembly)
                .Distinct();
            var publicMigrations =
                mappedAssemblies.SelectMany(a => a.GetExportedTypes())
                    .Where(t =>typeof (IMigration).IsAssignableFrom(t) &&t.GetConstructors().Any(c => c.GetParameters().Length == 0))
                    .Select(Activator.CreateInstance)
                    .Cast<IMigration>();

            //TODO: Handle backwards migration.
            var result = publicMigrations.Where(m => m.Version.CompareTo(current) > 0);
            if (targetVersion != null)
            {
                result = result.Where(m => m.Version.Version.CompareTo(targetVersion) <= 0);
            }
            return result;
        }

        private Configuration DeserializeConfiguration(byte[] data)
        {
            var ms = new MemoryStream(data);
            var gz = new GZipStream(ms, CompressionMode.Decompress);
            var f = new BinaryFormatter();
            return (Configuration)f.Deserialize(gz);

        }
        private byte[] SerializeConfiguration(Configuration configuration)
        {
            var ms = new MemoryStream();
            var gz = new GZipStream(ms, CompressionMode.Compress, true);
            BinaryFormatter f = new BinaryFormatter();
            f.Serialize(gz, configuration);
            gz.Close();
            gz.Dispose();
            ms.Flush();
            return ms.ToArray();
        }

      
    }
}