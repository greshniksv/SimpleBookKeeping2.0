
using Microsoft.EntityFrameworkCore;

namespace DAL.DbModels
{
	[Index(nameof(CostId))]
	public class CostDetail : BaseEntity
	{
		public DateTime Date { get; set; }

		public int Value { get; set; }

		public Guid CostId { get; set; }

		public Cost Cost { get; set; }

		public  bool Deleted {  get; set; }

		public ICollection<Spend> Spends { get; set; }
	}
}
