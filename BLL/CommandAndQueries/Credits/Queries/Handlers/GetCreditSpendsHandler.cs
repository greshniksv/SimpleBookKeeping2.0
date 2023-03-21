using System;
using System.Collections.Generic;
using System.Linq;
using BLL.CommandAndQueries.Credits.Queries;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Credits.Queries.Handlers
{
	public class GetCreditSpendsHandler : IRequestHandler<GetCreditSpends, IList<SpendCreditInfoModel>>
	{
		public IList<SpendCreditInfoModel> Handle(GetCreditSpends message)
		{
			if (message.CostId == Guid.Empty || message.UserId == Guid.Empty)
				throw new ArgumentNullException(nameof(message));

			IList<SpendCreditInfoModel> models;
			using (var session = Db.Session)
			{
				var data = string.Concat(DateTime.Now.ToString("yyyy-MM-dd"), " 00:00:00.000");
				var query = session.CreateSQLQuery("SELECT distinct {s.*}  " +
					  " FROM [CostDetails] as c, [Spends] as s\r\n  " +
					  " where c.[datetime] > \'" + data + "\'\r\n  and s.[cost_detail_id] = c.[id]\r\n  " +
					  " and c.[deleted] = 0 and c.[cost_id] = '" + message.CostId + "'");

				query.AddEntity("s", typeof(Spend));
				var items = query.List<Spend>();

				models = new List<SpendCreditInfoModel>();
				foreach (var spend in items)
				{
					var creditItem = models.FirstOrDefault(x => x.Comment == spend.Comment && x.Value == spend.Value);

					if (creditItem == null)
						models.Add(new SpendCreditInfoModel {
							Comment = spend.Comment,
							Value = spend.Value,
							DaysToFinish = 1
						});
					else
						creditItem.DaysToFinish++;
				}
			}

			return models;
		}
	}
}