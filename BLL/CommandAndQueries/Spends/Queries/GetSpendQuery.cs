using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Spends.Queries
{
	public class GetSpendQuery : IQuery<SpendModel>
	{
		public Guid SpendId { get; set; }
	}
}