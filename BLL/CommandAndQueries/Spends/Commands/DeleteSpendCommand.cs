using BLL.Interfaces;

namespace BLL.CommandAndQueries.Spends.Commands
{
	public class DeleteSpendCommand : ICommand<bool>
	{
		public Guid SpendId { get; set; }
	}
}
