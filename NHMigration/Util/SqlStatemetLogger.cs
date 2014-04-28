using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.AdoNet.Util;
using NHibernate.SqlCommand;

namespace NHMigration.Util
{
    /// <summary> Centralize logging handling for SQL statements. </summary>
    public class SqlStatementLogger
    {
        

        public Action<String> Log { get; set; }


    



        /// <summary> Log a IDbCommand. </summary>
        /// <param name="message">Title</param>
        /// <param name="command">The SQL statement. </param>
        /// <param name="style">The requested formatting style. </param>
        public virtual void LogCommand(string message, IDbCommand command, FormatStyle style)
        {
            if (string.IsNullOrEmpty(command.CommandText))
            {
                return;
            }

            string statement = style.Formatter.Format(GetCommandLineWithParameters(command));
            string logMessage;
            if (string.IsNullOrEmpty(message))
            {
                logMessage = statement;
            }
            else
            {
                logMessage = message + statement;
            }
            Log(logMessage);

        }

        public virtual void LogStatement(string message, SqlString sql, FormatStyle style)
        {
            var str = sql.ToString();
            if(String.IsNullOrEmpty(str)){ return;}

            var statement = style.Formatter.Format(str);
            if (string.IsNullOrWhiteSpace(message))
            {
                Log(statement);
            }
            else
            {
                Log(message + statement);
            }
        }

        /// <summary> Log a IDbCommand. </summary>
        /// <param name="command">The SQL statement. </param>
        /// <param name="style">The requested formatting style. </param>
        public virtual void LogCommand(IDbCommand command, FormatStyle style)
        {
            LogCommand(null, command, style);
        }

        public string GetCommandLineWithParameters(IDbCommand command)
        {
            string outputText;

            if (command.Parameters.Count == 0)
            {
                outputText = command.CommandText;
            }
            else
            {
                var output = new StringBuilder(command.CommandText.Length + (command.Parameters.Count * 20));
                output.Append(command.CommandText.TrimEnd(' ', ';', '\n'));
                output.Append(";");

                IDataParameter p;
                int count = command.Parameters.Count;
                bool appendComma = false;
                for (int i = 0; i < count; i++)
                {
                    if (appendComma)
                    {
                        output.Append(", ");
                    }
                    appendComma = true;
                    p = (IDataParameter)command.Parameters[i];
                    output.Append(string.Format("{0} = {1} [Type: {2}]", p.ParameterName, GetParameterLogableValue(p), GetParameterLogableType(p)));
                }
                outputText = output.ToString();
            }
            return outputText;
        }

        private static string GetParameterLogableType(IDataParameter dataParameter)
        {
            var p = dataParameter as IDbDataParameter;
            if (p != null)
                return p.DbType + " (" + p.Size + ")";
            return p.DbType.ToString();

        }

        public string GetParameterLogableValue(IDataParameter parameter)
        {
            const int maxLogableStringLength = 1000;
            if (parameter.Value == null || DBNull.Value.Equals(parameter.Value))
            {
                return "NULL";
            }
            if (IsStringType(parameter.DbType))
            {
                return string.Concat("'", TruncateWithEllipsis(parameter.Value.ToString(), maxLogableStringLength), "'");
            }
            var buffer = parameter.Value as byte[];
            if (buffer != null)
            {
                return GetBufferAsHexString(buffer);
            }
            return parameter.Value.ToString();
        }

        private static string GetBufferAsHexString(byte[] buffer)
        {
            const int maxBytes = 128;
            int bufferLength = buffer.Length;

            var sb = new StringBuilder(maxBytes * 2 + 8);
            sb.Append("0x");
            for (int i = 0; i < bufferLength && i < maxBytes; i++)
            {
                sb.Append(buffer[i].ToString("X2"));
            }
            if (bufferLength > maxBytes)
            {
                sb.Append("...");
            }
            return sb.ToString();
        }

        private static bool IsStringType(DbType dbType)
        {
            return DbType.String.Equals(dbType) || DbType.AnsiString.Equals(dbType)
                         || DbType.AnsiStringFixedLength.Equals(dbType) || DbType.StringFixedLength.Equals(dbType);
        }



        public void LogBatchCommand(string batchCommand)
        {
            Log(batchCommand);
 
        }

        private string TruncateWithEllipsis(string source, int length)
        {
            const string ellipsis = "...";
            if (source.Length > length)
            {
                return source.Substring(0, length) + ellipsis;
            }
            return source;
        }
    }
}
