using System.Runtime.Serialization;

namespace BLL.Exceptions.GetUserQuery
{
	[Serializable]
	public class GetUserQueryException : Exception
	{
		public GetUserQueryException()
		{
		}

		public GetUserQueryException(string message)
			: base(message)
		{
		}

		public GetUserQueryException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected GetUserQueryException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}
