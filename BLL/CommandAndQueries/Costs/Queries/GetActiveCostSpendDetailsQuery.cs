using BLL.DtoModels;
using MediatR;

namespace BLL.CommandAndQueries.Costs.Queries
{
	public class GetActiveCostSpendDetailsQuery : IRequest<IList<CostSpendDetailModel>>
	{
		public Guid UserId { get; set; }

		public Guid CostId { get; set; }
	}
}