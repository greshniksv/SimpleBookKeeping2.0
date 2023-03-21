using System;
using MediatR;

namespace BLL.CommandAndQueries.Plans.Commands
{
	public class RemovePlanCommand : IRequest<bool>
	{
		public Guid PlanId { get; set; }
	}
}