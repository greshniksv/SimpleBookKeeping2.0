using System.Runtime.Serialization;

namespace BLL.Exceptions.CreateUser
{
	[Serializable]
	public class CreateUserException : Exception
	{
		public CreateUserException()
		{
		}

		public CreateUserException(string message)
			: base(message)
		{
		}

		public CreateUserException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected CreateUserException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}
