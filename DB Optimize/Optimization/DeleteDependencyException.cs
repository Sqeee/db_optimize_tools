using System;
using System.Runtime.Serialization;

namespace DB_Optimize.Optimization
{
    public class DeleteDependencyException : Exception
    {
        public DeleteDependencyException()
        {
        }

        public DeleteDependencyException(string message) : base(message)
        {
        }

        public DeleteDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeleteDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
