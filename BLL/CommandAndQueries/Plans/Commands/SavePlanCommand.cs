using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Plans.Commands
{
	public class SavePlanCommand : ICommand<bool>
	{
		public PlanModel PlanModel { get; set; }

		public Guid UserId { get; set; }
	}
}