using System.Runtime.Serialization;

namespace BLL.Exceptions
{
	[Serializable]
	public class HttpContextServiceException : Exception
	{
		public HttpContextServiceException()
		{
		}

		public HttpContextServiceException(string message)
			: base(message)
		{
		}

		public HttpContextServiceException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected HttpContextServiceException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}
