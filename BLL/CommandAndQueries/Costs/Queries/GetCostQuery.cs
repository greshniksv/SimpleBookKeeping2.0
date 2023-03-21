using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Costs.Queries
{
	public class GetCostQuery : IQuery<CostModel>
	{
		public Guid CostId { get; set; }
	}
}