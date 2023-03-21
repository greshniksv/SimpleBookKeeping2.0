using System;
using System.Collections.Generic;
using BLL.CommandAndQueries.Spends.Queries;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Spends.Queries.Handlers
{
	public class GetSpendsQueryHandler : IRequestHandler<GetSpendsQuery, IList<SpendModel>>
	{
		/// <summary>Handles a request</summary>
		/// <param name="message">The request message</param>
		/// <returns>Response from the request</returns>
		public IList<SpendModel> Handle(GetSpendsQuery message)
		{
			if (message.CostId == Guid.Empty || message.UserId == Guid.Empty)
				throw new ArgumentNullException(nameof(message));

			IList<SpendModel> models;
			using (var session = Db.Session)
			{
				var items = session.QueryOver<Spend>()
					.Where(x => x.User.Id == message.UserId && x.CostDetail.Cost.Id == message.CostId).List();

				models = AutoMapperConfig.Mapper.Map<IList<SpendModel>>(items);
			}

			return models;
		}
	}
}