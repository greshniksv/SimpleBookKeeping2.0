using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediatR;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Plans.Queries
{
	public class GetActivePlanCostsQuery : IRequest<IReadOnlyCollection<PlanCostsModel>>
	{
		public Guid UserId { get; set; }
	}
}