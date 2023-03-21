using System.Collections.Generic;
using System.Linq;
using BLL.CommandAndQueries.Plans.Queries;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Exceptions;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetPlanQueryHandler : IRequestHandler<GetPlanQuery, PlanModel>
	{
		public PlanModel Handle(GetPlanQuery message)
		{
			PlanModel model;
			using (var session = Db.Session)
			{
				var plans = session.QueryOver<Plan>()
					.Where(p => p.Id == message.PlanId && p.Deleted == false).List();

				if (!plans.Any())
					throw new PlanNotFoundException($"Plan id: {message.PlanId}");

				model = AutoMapperConfig.Mapper.Map<PlanModel>(plans.First());
			}

			return model;
		}
	}
}