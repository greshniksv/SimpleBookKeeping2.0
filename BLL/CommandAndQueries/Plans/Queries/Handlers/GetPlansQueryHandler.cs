using System;
using System.Collections.Generic;
using System.Linq;
using BLL.CommandAndQueries.Plans.Queries;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetPlansQueryHandler : IRequestHandler<GetPlansQuery, IList<PlanModel>>
	{
		public IList<PlanModel> Handle(GetPlansQuery message)
		{
			if (message.UserId == Guid.Empty)
				throw new Exception("GetPlansQueryHandler. UserId can't be empty.");

			IList<PlanModel> planModels;
			using (var session = Db.Session)
			{
				List<Plan> plans = new List<Plan>();
				var planQuery = session.QueryOver<Plan>();

				planQuery = planQuery.Where(x => x.User.Id == message.UserId);

				if (message.ShowDeleted != null)
					planQuery = planQuery.Where(x => x.Deleted == message.ShowDeleted);

				if (message.IsActive != null)
				{
					var now = DateTime.Now;
					planQuery = planQuery.Where(x => x.Start <= now && x.End >= now);
				}

				plans.AddRange(planQuery.List());

				List<Plan> planByMember =
					session.QueryOver<PlanMember>().Where(x => x.User.Id == message.UserId).Select(x => x.Plan).List<Plan>().ToList();

				if (message.ShowDeleted != null)

					planByMember.RemoveAll(x => x.Deleted != message.ShowDeleted);

				if (message.IsActive != null)
				{
					var now = DateTime.Now;
					planByMember.RemoveAll(x => !(x.Start <= now && x.End >= now));
				}

				plans.AddRange(planByMember);

				planModels = AutoMapperConfig.Mapper.Map<IList<PlanModel>>(plans.Distinct());
			}

			return planModels;
		}
	}
}