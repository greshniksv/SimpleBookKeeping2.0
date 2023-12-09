using BLL.DtoModels;
using MediatR;

namespace BLL.CommandAndQueries.Plans.Queries
{
	public class GetActivePlanCostsQuery : IRequest<IReadOnlyCollection<PlanCostsModel>>
	{
		public Guid UserId { get; set; }
	}
}