using System;
using System.Runtime.Serialization;

namespace FactoriesManager.Exceptions
{
    [Serializable]
    public class ClosedReportException : InvalidOperationException
    {
        public ClosedReportException() : base("Report is closed.")
        {
        }

        public ClosedReportException(string message) : base(message)
        {
        }

        public ClosedReportException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClosedReportException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}