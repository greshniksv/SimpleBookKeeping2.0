using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Spends.Queries
{
	public class GetSpendsQuery : IQuery<IReadOnlyList<SpendModel>>
	{
		public Guid UserId { get; set; }

		public Guid CostId { get; set; }
	}
}