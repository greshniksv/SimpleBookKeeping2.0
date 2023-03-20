
using Microsoft.EntityFrameworkCore;

namespace DAL.DbModels
{
	[Index(nameof(UserId))]
	[Index(nameof(CostDetailId))]
	public class Spend : BaseEntity
	{
		public Guid UserId { get; set; }

		public User User { get; set; }

		public Guid CostDetailId { get; set; }

		public CostDetail CostDetail { get; set; }

		public int OrderId { get; set; }

		public int Value { get; set; }

		public string Comment { get; set; }

		public string Image { get; set; }
	}
}
