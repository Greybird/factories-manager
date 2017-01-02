using System;
using System.Runtime.Serialization;

namespace FactoriesManager.Exceptions
{
    [Serializable]
    public class OutdatedReportIdException : InvalidOperationException
    {
        public OutdatedReportIdException() : base("Report Id is outdated.")
        {
        }

        public OutdatedReportIdException(string message) : base(message)
        {
        }

        public OutdatedReportIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutdatedReportIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}