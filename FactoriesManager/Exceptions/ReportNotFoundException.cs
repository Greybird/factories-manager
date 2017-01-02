using System;
using System.Runtime.Serialization;

namespace FactoriesManager.Exceptions
{
    [Serializable]
    public class ReportNotFoundException : Exception
    {
        public ReportNotFoundException() : base("Report not found.")
        {
        }

        public ReportNotFoundException(string message) : base(message)
        {
        }

        public ReportNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReportNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}