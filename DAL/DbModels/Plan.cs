
using Microsoft.EntityFrameworkCore;

namespace DAL.DbModels
{
	[Index(nameof(UserId))]
	[Index(nameof(UserId), nameof(Start), nameof(End))]
	public class Plan : BaseEntity
	{
		public string Name { get; set; }

		public DateTime Start { get; set; }

		public DateTime End { get; set; }

		public int Balance { get; set; }

		public Guid UserId { get; set; }

		public User User { get; set; }

		public bool Deleted { get; set; }

		public ICollection<PlanMember> PlanMembers { get; set; }

		public ICollection<Cost> Costs { get; set; }
	}
}
