using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Credits.Commands
{
	public class SaveCreditSpendCommand : ICommand<bool>
	{
		public IReadOnlyCollection<AddCreditSpendModel> SpendModels { get; set; }

		public Guid UserId { get; set; }
	}
}