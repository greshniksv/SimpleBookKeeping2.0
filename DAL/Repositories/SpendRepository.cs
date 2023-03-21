using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
	public class SpendRepository : BaseRepository<Spend>, ISpendRepository
	{
		public SpendRepository(IMainContext context)
			: base(context)
		{
		}
	}
}
