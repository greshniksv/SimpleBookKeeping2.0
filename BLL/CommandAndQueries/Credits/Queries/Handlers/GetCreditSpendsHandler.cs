using BLL.DtoModels;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.CommandAndQueries.Credits.Queries.Handlers
{
	public class GetCreditSpendsHandler : IQueryHandler<GetCreditSpends, IReadOnlyList<SpendCreditInfoModel>>
	{
		private readonly IMainContext _mainContext;

		public GetCreditSpendsHandler(IMainContext mainContext)
		{
			_mainContext = mainContext;
		}

		public async Task<IReadOnlyList<SpendCreditInfoModel>> Handle(GetCreditSpends request, CancellationToken cancellationToken)
		{
			if (request.CostId == Guid.Empty || request.UserId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(request));
			}

			List<Spend> items = await (from costDetails in _mainContext.CostDetails
									   join spend in _mainContext.Spends on costDetails.Id equals spend.CostDetailId
									   where costDetails.Date > DateTime.Now &&
									         costDetails.CostId == request.CostId &&
											 costDetails.Deleted == false
									   select spend).Distinct().ToListAsync(cancellationToken);

			//var query = session.CreateSQLQuery("SELECT distinct {s.*}  " +
			//   " FROM [CostDetails] as c, [Spends] as s\r\n  " +
			//   " where c.[datetime] > \'" + data + "\'\r\n  and s.[cost_detail_id] = c.[id]\r\n  " +
			//   " and c.[deleted] = 0 and c.[cost_id] = '" + request.CostId + "'");

			IList<SpendCreditInfoModel> models = new List<SpendCreditInfoModel>();
			foreach (var spend in items)
			{
				SpendCreditInfoModel? creditItem =
					models.FirstOrDefault(x => x.Comment == spend.Comment && x.Value == spend.Value);

				if (creditItem == null)
				{
					models.Add(new SpendCreditInfoModel {
						Comment = spend.Comment,
						Value = spend.Value,
						DaysToFinish = 1
					});
				}
				else
				{
					creditItem.DaysToFinish++;
				}
			}


			return models.AsReadOnly();
		}
	}
}