using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Spends.Commands
{
	public class SaveSpendCommand : ICommand<bool>
	{
		public IReadOnlyCollection<AddSpendModel> SpendModels { get; set; }

		public Guid UserId { get; set; }
	}
}