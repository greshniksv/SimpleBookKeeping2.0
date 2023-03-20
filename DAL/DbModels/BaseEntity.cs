using System.ComponentModel.DataAnnotations;

namespace DAL.DbModels
{
	public abstract class BaseEntity
	{
		[Key]
		public Guid Id { get; protected set; }
	}
}
