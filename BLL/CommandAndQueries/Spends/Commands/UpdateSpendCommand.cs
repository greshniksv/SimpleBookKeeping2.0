using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Spends.Commands
{
	public class UpdateSpendCommand : ICommand<bool>
	{
		public AddSpendModel SpendModel { get; set; }

		public Guid UserId { get; set; }
	}
}