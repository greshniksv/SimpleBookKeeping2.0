using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		public UserRepository(IMainContext context)
			: base(context)
		{
		}
	}
}
