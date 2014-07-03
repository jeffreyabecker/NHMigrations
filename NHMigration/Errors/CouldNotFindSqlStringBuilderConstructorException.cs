using System;

namespace NHMigration.Errors
{
    public class CouldNotFindSqlStringBuilderConstructorException : Exception
    {
        public CouldNotFindSqlStringBuilderConstructorException(Type type) 
            : base(String.Format("Couldnt figure out how to construct a {0}", type.FullName)){}

    }
}