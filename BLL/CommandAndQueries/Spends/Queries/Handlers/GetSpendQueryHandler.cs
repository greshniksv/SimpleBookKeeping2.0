using System.Linq;
using BLL.CommandAndQueries.Spends.Queries;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Spends.Queries.Handlers
{
	public class GetSpendQueryHandler : IRequestHandler<GetSpendQuery, SpendModel>
	{
		public SpendModel Handle(GetSpendQuery message)
		{
			using (var session = Db.Session)
			{
				Spend spend = session.QueryOver<Spend>().Where(x => x.Id == message.SpendId).List().First();
				var spendModel = AutoMapperConfig.Mapper.Map<SpendModel>(spend);
				return spendModel;
			}
		}
	}
}