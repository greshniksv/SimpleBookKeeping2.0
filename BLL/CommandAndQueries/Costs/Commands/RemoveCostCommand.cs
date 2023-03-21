using BLL.Interfaces;

namespace BLL.CommandAndQueries.Costs.Commands
{
	public class RemoveCostCommand : ICommand<bool>
	{
		public Guid CostId { get; set; }
	}
}