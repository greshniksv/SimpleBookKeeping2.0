using System;
using System.Runtime.Serialization;

namespace SimpleBookKeeping.Exceptions
{
    [Serializable]
    public class PlanNotFoundException : Exception
    {
        public PlanNotFoundException()
        {
        }

        public PlanNotFoundException(string message) : base(message)
        {
        }

        public PlanNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlanNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}