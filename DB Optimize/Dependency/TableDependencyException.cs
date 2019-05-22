using System;
using System.Runtime.Serialization;

namespace DB_Optimize.Dependency
{
    public class TableDependencyException : Exception
    {
        public TableDependencyException()
        {
        }

        public TableDependencyException(string message) : base(message)
        {
        }

        public TableDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TableDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
