using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.AdoNet.Util;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHMigration.Errors;
using NHMigration.Versioning;

namespace NHMigration
{
    public enum MigrationMode {ScriptOnly, ScriptAndExecute}
    public class MigrationDatabaseContex : IDisposable
    {
        private readonly ISessionFactoryImplementor _sessionFactoryImplementor;
        private readonly  NHMigration.Util.SqlStatementLogger _logger = new  NHMigration.Util.SqlStatementLogger();

        public MigrationDatabaseContex(ISessionFactoryImplementor sessionFactoryImplementor, MigrationMode mode, string migrationContextName = "NHMigration", Action<string> log = null)
        {
            _sessionFactoryImplementor = sessionFactoryImplementor;
            Mode = mode;
            Name = migrationContextName;
            Logger = log ?? Console.Write;
            Connection = _sessionFactoryImplementor.ConnectionProvider.GetConnection();
        }
        public MigrationMode Mode { get; protected set; }


        public string Name { get; protected set; }
        public Dialect Dialect { get { return _sessionFactoryImplementor.Dialect; } }
        public IDriver Driver { get { return _sessionFactoryImplementor.ConnectionProvider.Driver; } }
        public IDbConnection Connection { get; protected set; }
        public Action<string> Logger
        {
            get { return _logger.Log; }
            set { _logger.Log = value; }
        }

        public void Log(IDbCommand cmd, FormatStyle style = null)
        {
            _logger.LogCommand(cmd, style?? FormatStyle.Basic);
        }

        public void Log(SqlString sql, FormatStyle style= null)
        {
            _logger.LogStatement(null, sql, style ?? FormatStyle.Basic);
        }



        public IDbCommand GenerateCommand(SqlCommandInfo info)
        {
            var cmd = Driver.GenerateCommand(info.CommandType, info.Text, info.ParameterTypes);
            cmd.Connection = Connection;
            return cmd;
        }
        public IDbCommand GenerateCommand(SqlString query, params SqlType[] parameterTypes)
        {
            var cmd = Driver.GenerateCommand(CommandType.Text, query, parameterTypes);
            cmd.Connection = Connection;
            return cmd;
        }

        
        public T CreateSqlBuilder<T>() where T: ISqlStringBuilder
        {
            var cp = typeof (T).GetConstructors()
                .Select(c => new
                {
                    ConstructorInfo = c,
                    Parameters = c.GetParameters()
                })
                .ToList();

            var try1 = cp.FirstOrDefault(a=>a.Parameters.Length == 1 && a.Parameters[0].ParameterType == typeof(ISessionFactoryImplementor));
            if (try1 != null)
            {
                return (T) try1.ConstructorInfo.Invoke(new object[] {_sessionFactoryImplementor});
            }
            var try2 = cp.FirstOrDefault(a=>a.Parameters.Length == 2 && a.Parameters[0].ParameterType == typeof(Dialect) && a.Parameters[1].ParameterType == typeof(IMapping));
            if (try2 != null)
            {
                return (T) try2.ConstructorInfo.Invoke(new object[]{_sessionFactoryImplementor.Dialect, (IMapping)_sessionFactoryImplementor});
            }
   
            throw new CouldNotFindSqlStringBuilderConstructorException(typeof(T));
        }


        public void Dispose()
        {
            Connection.Dispose();
            
        }

        public IDictionary<string, IClassMetadata> GetAllClassMetatdata()
        {
            return _sessionFactoryImplementor.GetAllClassMetadata();
        }
    }
}