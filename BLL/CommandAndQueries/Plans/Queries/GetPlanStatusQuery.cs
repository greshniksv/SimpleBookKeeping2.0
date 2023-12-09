using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Plans.Queries
{
	public class GetPlanStatusQuery : IQuery<PlanStatusModel>
	{
		public Guid PlanId { get; set; }
	}
}