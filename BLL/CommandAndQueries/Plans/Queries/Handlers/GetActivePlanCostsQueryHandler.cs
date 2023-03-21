using System.Collections.Generic;
using System.Linq;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetActivePlanCostsQueryHandler : IRequestHandler<GetActivePlanCostsQuery, IReadOnlyCollection<PlanCostsModel>>
	{
		/// <summary>Handles a request</summary>
		/// <param name="message">The request message</param>
		/// <returns>Response from the request</returns>
		public IReadOnlyCollection<PlanCostsModel> Handle(GetActivePlanCostsQuery message)
		{
			List<PlanCostsModel> planCostsModels = new List<PlanCostsModel>();
			List<Plan> plans = new List<Plan>();
			using (var session = Db.Session)
			{
				// Note: Find by creator and by member in plan.
				var plansByCreator = session.QueryOver<Plan>().Where(x => x.User.Id == message.UserId && x.Deleted == false).List<Plan>();
				var plansByMember = session.QueryOver<PlanMember>().Where(x => x.User.Id == message.UserId).Select(x => x.Plan).List<Plan>();

				plans.AddRange(plansByCreator);
				plans.AddRange(plansByMember.Where(x => x.Deleted == false));

				planCostsModels.AddRange(AutoMapperConfig.Mapper.Map<List<PlanCostsModel>>(plans.Distinct()));
			}

			return planCostsModels;
		}
	}
}