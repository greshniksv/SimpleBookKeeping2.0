using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
	public class CostRepository : BaseRepository<Cost>, ICostRepository
	{
		public CostRepository(IMainContext context)
			: base(context)
		{
		}
	}
}
