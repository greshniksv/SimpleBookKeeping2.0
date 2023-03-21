using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
	public class PlanMemberRepository : BaseRepository<PlanMember>, IPlanMemberRepository
	{
		public PlanMemberRepository(IMainContext context)
			: base(context)
		{
		}
	}
}
