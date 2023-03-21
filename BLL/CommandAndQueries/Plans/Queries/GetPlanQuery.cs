using System;
using MediatR;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Queries
{
	public class GetPlanQuery : IRequest<PlanModel>
	{
		public Guid PlanId { get; set; }
	}
}