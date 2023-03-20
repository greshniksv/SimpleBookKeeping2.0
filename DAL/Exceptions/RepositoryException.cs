using System.Runtime.Serialization;

namespace DAL.Exceptions
{
	[Serializable]
	public class RepositoryException : Exception
	{
		public RepositoryException()
			: base()
		{
		}

		public RepositoryException(string? message)
			: base(message)
		{
		}

		public RepositoryException(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}

		protected RepositoryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
