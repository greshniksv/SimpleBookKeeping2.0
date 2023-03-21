using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Costs.Commands
{
	public class CreateCostCommand : ICommand<CostModel>
	{
		public Guid PlanId { get; set; }
	}
}