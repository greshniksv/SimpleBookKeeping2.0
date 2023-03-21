using System.Collections.Generic;
using System.Linq;
using BLL.CommandAndQueries.Plans.Commands;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;

namespace BLL.CommandAndQueries.Plans.Commands.Handlers
{
	public class SavePlanCommandHandler : IRequestHandler<SavePlanCommand, bool>
	{
		/// <summary>Handles a request</summary>
		/// <param name="message">The request message</param>
		/// <returns>Response from the request</returns>
		public bool Handle(SavePlanCommand message)
		{
			IList<PlanMember> existingPlanMembers = null;
			Plan plan;
			IList<User> users;
			using (var session = Db.Session)
			{
				users = session.QueryOver<User>().List();

				plan = session.QueryOver<Plan>()
			   .Where(p => p.Id == message.PlanModel.Id).List().FirstOrDefault();

				var currentUser = session.QueryOver<User>()
					.Where(x => x.Id == message.UserId)
					.List().FirstOrDefault();

				if (plan == null)
					plan = new Plan {
						User = currentUser
					};
				else
					// Get PlanMembers and remove it
					existingPlanMembers = plan.PlanMembers.ToList();
			}

			AutoMapperConfig.Mapper.Map(message.PlanModel, plan);

			using (var session = Db.Session)
			using (var transaction = session.BeginTransaction())
			{
				// Add plan
				session.SaveOrUpdate(plan);

				// Remove old plan members
				if (existingPlanMembers != null && existingPlanMembers.Any())
					foreach (var existingPlanMember in existingPlanMembers)
					{
						existingPlanMember.User = null;
						existingPlanMember.Plan = null;
						session.Delete(existingPlanMember);
					}

				// Add plan members
				foreach (var userMember in message.PlanModel.UserMembers)
				{
					var user = users.First(x => x.Id == userMember);
					session.Save(new PlanMember { User = user, Plan = plan });
				}

				transaction.Commit();
			}

			return true;
		}
	}
}