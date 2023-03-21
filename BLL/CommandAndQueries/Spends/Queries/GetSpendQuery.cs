using System;
using MediatR;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Spends.Queries
{
	public class GetSpendQuery : IRequest<SpendModel>
	{
		public Guid SpendId { get; set; }
	}
}