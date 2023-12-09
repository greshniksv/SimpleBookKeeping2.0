using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Users.Queries
{
	public class GetUsersQuery : IQuery<IReadOnlyList<UserModel>>
	{
		public IList<Guid>? UsersId { get; set; }
	}
}