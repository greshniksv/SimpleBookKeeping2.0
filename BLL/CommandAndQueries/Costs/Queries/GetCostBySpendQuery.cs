using BLL.Interfaces;

namespace BLL.CommandAndQueries.Costs.Queries
{
	public class GetCostBySpendQuery : IQuery<Guid?>
	{
		public Guid SpendId { get; set; }
	}
}