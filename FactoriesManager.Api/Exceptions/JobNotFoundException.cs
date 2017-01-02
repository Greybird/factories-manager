using System;
using System.Runtime.Serialization;

namespace FactoriesManager.Api.Exceptions
{
    [Serializable]
    public class JobNotFoundException : Exception
    {
        public JobNotFoundException() : base("Job not found.")
        {
        }

        public JobNotFoundException(string message) : base(message)
        {
        }

        public JobNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JobNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}