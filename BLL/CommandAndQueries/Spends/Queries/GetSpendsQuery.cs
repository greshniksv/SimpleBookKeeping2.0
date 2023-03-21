using System;
using System.Collections.Generic;
using MediatR;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Spends.Queries
{
	public class GetSpendsQuery : IRequest<IList<SpendModel>>
	{
		public Guid UserId { get; set; }

		public Guid CostId { get; set; }
	}
}