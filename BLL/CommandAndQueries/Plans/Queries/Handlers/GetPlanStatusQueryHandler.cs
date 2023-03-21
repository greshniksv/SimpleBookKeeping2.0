using System;
using System.Collections.Generic;
using System.Linq;
using BLL.CommandAndQueries.Plans.Queries;
using MediatR;
using NHibernate;
using NHibernate.Transform;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetPlanStatusQueryHandler : IRequestHandler<GetPlanStatusQuery, PlanStatusModel>
	{

		/// <summary>Handles a request</summary>
		/// <param name="message">The request message</param>
		/// <returns>Response from the request</returns>
		public PlanStatusModel Handle(GetPlanStatusQuery message)
		{
			PlanStatusModel planStatusModel = new PlanStatusModel();
			List<CostStatusModel> costStatusModels = new List<CostStatusModel>();
			using (var session = Db.Session)
			{
				int allSpend = 0;
				var plan = session.QueryOver<Plan>().Where(x => x.Id == message.PlanId).List().FirstOrDefault();
				if (plan == null)
					throw new Exception("GetPlanStatusQuery. Plan not found.");
				//var costs = plan.Costs.Where(x => x.Deleted == false).ToList();

				var passedDays = (DateTime.Now.Date - plan.Start.Date).Days;
				var totalDays = (plan.End.Date - plan.Start.Date).Days;

				planStatusModel.Id = plan.Id;
				planStatusModel.Name = plan.Name;
				planStatusModel.Progress = passedDays * 100 / totalDays;

				var list = session.CreateSQLQuery($"exec dbo.CostList @Plan='{plan.Id}'")
					.SetResultTransformer(Transformers.AliasToBean<CostStatusModel>()).List<CostStatusModel>();

				var allSpends = session.CreateSQLQuery($"exec [dbo].[SpendsSumByPlan] @Plan='{plan.Id}'")
					.AddScalar("Sum", NHibernateUtil.Int32).List<int>();

				costStatusModels.AddRange(list.OrderBy(x => x.Name).ToList());
				// Balance on start minus sum of planed costs
				planStatusModel.Rest = plan.Balance - allSpends.First();
				planStatusModel.Balance = costStatusModels.Sum(x => x.Balance);
			}

			planStatusModel.CostStatusModels = costStatusModels;
			return planStatusModel;
		}
	}
}