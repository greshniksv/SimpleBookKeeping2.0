using System.Runtime.Serialization;

namespace DAL.Exceptions
{
	[Serializable]
	public class RepositorySqlException : RepositoryException
	{
		public RepositorySqlException()
			: base()
		{
		}

		public RepositorySqlException(string? message)
			: base(message)
		{
		}

		public RepositorySqlException(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}

		protected RepositorySqlException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
