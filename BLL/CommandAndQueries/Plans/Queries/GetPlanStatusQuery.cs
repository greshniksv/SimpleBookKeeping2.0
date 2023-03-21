using System;
using MediatR;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Queries
{
	public class GetPlanStatusQuery : IRequest<PlanStatusModel>
	{
		public Guid PlanId { get; set; }
	}
}