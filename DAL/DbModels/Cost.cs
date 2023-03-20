using Microsoft.EntityFrameworkCore;

namespace DAL.DbModels
{
	[Index(nameof(PlanId))]
	public class Cost : BaseEntity
	{
		public string Name { get; set; }

		public Guid PlanId { get; set; }

		public Plan Plan { get; set; }

		public bool Deleted { get; set; }

		public ICollection<CostDetail> CostDetails { get; set; }
	}
}
