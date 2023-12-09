using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Plans.Queries
{
	public class GetPlanQuery : IQuery<PlanModel>
	{
		public Guid PlanId { get; set; }
	}
}