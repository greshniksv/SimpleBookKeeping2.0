using System;
using System.Runtime.Serialization;

namespace SimpleBookKeeping.Exceptions
{
    [Serializable]
    public class CookieDecryptException : Exception
    {
        public CookieDecryptException()
        {
        }

        public CookieDecryptException(string message) : base(message)
        {
        }

        public CookieDecryptException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CookieDecryptException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}