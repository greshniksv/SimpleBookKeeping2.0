using BLL.Interfaces;

namespace BLL.CommandAndQueries.Plans.Commands
{
	public class RemovePlanCommand : ICommand<bool>
	{
		public Guid PlanId { get; set; }
	}
}