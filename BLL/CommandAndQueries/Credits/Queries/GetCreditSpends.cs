using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Credits.Queries
{
	public class GetCreditSpends : IQuery<IReadOnlyList<SpendCreditInfoModel>>
	{
		public Guid UserId { get; set; }
		public Guid CostId { get; set; }
	}
}