using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
	public class CostDetailRepository : BaseRepository<CostDetail>, ICostDetailRepository
	{
		public CostDetailRepository(IMainContext context)
			: base(context)
		{
		}
	}
}
