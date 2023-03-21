using System.Collections.Generic;
using BLL.CommandAndQueries.Users.Queries;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Models;

namespace BLL.CommandAndQueries.Users.Queries.Handlers
{
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IList<UserModel>>
	{
		/// <summary>Handles a request</summary>
		/// <param name="message">The request message</param>
		/// <returns>Response from the request</returns>
		public IList<UserModel> Handle(GetUsersQuery message)
		{
			IList<UserModel> userModels;
			using (var session = Db.Session)
			{
				var users = session.QueryOver<User>();
				if (message.UsersId != null)
					users.Where(x => message.UsersId.Contains(x.Id));
				userModels = AutoMapperConfig.Mapper.Map<IList<UserModel>>(users.List());
			}

			return userModels;
		}
	}
}