using System;
using System.Runtime.Serialization;

namespace SimpleBookKeeping.Exceptions
{
    [Serializable]
    public class CostNotFoundException : Exception
    {
        public CostNotFoundException()
        {
        }

        public CostNotFoundException(string message) : base(message)
        {
        }

        public CostNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CostNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}