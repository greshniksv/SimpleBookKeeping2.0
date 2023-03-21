using System;
using System.Collections.Generic;
using MediatR;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Credits.Commands
{
	public class SaveCreditSpendCommand : IRequest
	{
		public IReadOnlyCollection<AddCreditSpendModel> SpendModels { get; set; }

		public Guid UserId { get; set; }
	}
}