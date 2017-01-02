using System;
using System.Runtime.Serialization;

namespace FactoriesManager.Exceptions
{
    [Serializable]
    public class ResultsNotFoundException : Exception
    {
        public ResultsNotFoundException() : base("Results not found.")
        {
        }

        public ResultsNotFoundException(string message) : base(message)
        {
        }

        public ResultsNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ResultsNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}