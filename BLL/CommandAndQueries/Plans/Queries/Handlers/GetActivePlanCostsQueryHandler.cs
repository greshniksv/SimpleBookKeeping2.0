using AutoMapper;
using BLL.DtoModels;
using DAL.DbModels;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetActivePlanCostsQueryHandler : IRequestHandler<GetActivePlanCostsQuery, IReadOnlyCollection<PlanCostsModel>>
	{
		private readonly IPlanRepository _planRepository;
		private readonly IPlanMemberRepository _memberRepository;
		private readonly IMapper _mapper;

		public GetActivePlanCostsQueryHandler(IPlanRepository planRepository, IPlanMemberRepository memberRepository, IMapper mapper)
		{
			_planRepository = planRepository;
			_memberRepository = memberRepository;
			_mapper = mapper;
		}

		/// <summary>Handles a request</summary>
		/// <param name="request">The request message</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Response from the request</returns>
		public async Task<IReadOnlyCollection<PlanCostsModel>> Handle(GetActivePlanCostsQuery request, CancellationToken cancellationToken)
		{
			List<PlanCostsModel> planCostsModels = new();
			List<Plan> plans = new List<Plan>();
			List<Plan> plansByCreator =
				await _planRepository.GetAsync(x => x.User.Id == request.UserId && x.Deleted == false).ToListAsync(cancellationToken);
			IEnumerable<Plan> plansByMember =
				(await _memberRepository.GetAsync(x =>
					x.User.Id == request.UserId, null, $"{nameof(PlanMember.Plan)}").ToListAsync(cancellationToken))
				.Select(x => x.Plan);

			plans.AddRange(plansByCreator);
			plans.AddRange(plansByMember.Where(x => x.Deleted == false));

			planCostsModels.AddRange(_mapper.Map<List<PlanCostsModel>>(plans.Distinct()));

			return planCostsModels;
		}
	}
}