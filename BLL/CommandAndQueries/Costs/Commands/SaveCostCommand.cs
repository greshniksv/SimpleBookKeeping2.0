using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Costs.Commands
{
	public class SaveCostCommand : ICommand<bool>
	{
		public CostModel Cost { get; set; }
	}
}