using System;
using MediatR;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Commands
{
	public class SavePlanCommand : IRequest<bool>
	{
		public PlanModel PlanModel { get; set; }

		public Guid UserId { get; set; }
	}
}