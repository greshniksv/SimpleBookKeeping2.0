using AutoMapper;
using BLL.DtoModels;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetPlansQueryHandler : IQueryHandler<GetPlansQuery, IList<PlanModel>>
	{
		private readonly IPlanRepository _planRepository;
		private readonly IPlanMemberRepository _memberRepository;
		private readonly IMainContext _mainContext;
		private readonly IMapper _mapper;

		public GetPlansQueryHandler(IPlanRepository planRepository, IPlanMemberRepository memberRepository, IMapper mapper, IMainContext mainContext)
		{
			_planRepository = planRepository;
			_memberRepository = memberRepository;
			_mapper = mapper;
			_mainContext = mainContext;
		}

		public async Task<IList<PlanModel>> Handle(GetPlansQuery request, CancellationToken cancellationToken)
		{
			if (request.UserId == Guid.Empty)
			{
				throw new Exception("GetPlansQueryHandler. UserId can't be empty.");
			}

			List<Plan> plans = new List<Plan>();
			IQueryable<Plan> planQuery = _mainContext.Plans.AsQueryable();
			planQuery = planQuery.Where(x => x.UserId == request.UserId);

			if (request.ShowDeleted != null)
			{
				planQuery = planQuery.Where(x => x.Deleted == request.ShowDeleted);
			}

			if (request.IsActive != null)
			{
				DateTime now = DateTime.Now;
				planQuery = planQuery.Where(x => x.Start <= now && x.End >= now);
			}

			plans.AddRange(await planQuery.ToListAsync(cancellationToken));
			List<Plan> planByMember =
				await _memberRepository.GetAsync(x =>
					x.UserId == request.UserId, null,
						$"{nameof(PlanMember.Plan)}")
					.Select(x => x.Plan)
					.ToListAsync(cancellationToken);

			if (request.ShowDeleted != null)
			{
				planByMember.RemoveAll(x => x.Deleted != request.ShowDeleted);
			}

			if (request.IsActive != null)
			{
				DateTime now = DateTime.Now;
				planByMember.RemoveAll(x => !(x.Start <= now && x.End >= now));
			}

			plans.AddRange(planByMember);
			IList<PlanModel> planModels = _mapper.Map<IList<PlanModel>>(plans.Distinct());

			return planModels;
		}
	}
}