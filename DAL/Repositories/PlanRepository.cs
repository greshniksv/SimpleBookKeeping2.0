using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
	public class PlanRepository : BaseRepository<Plan>, IPlanRepository
	{
		public PlanRepository(IMainContext context)
			: base(context)
		{
		}
	}
}
