using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Costs.Commands
{
	public class GenerateCostCommand : ICommand<CostModel>
	{
		public Guid PlanId { get; set; }
	}
}