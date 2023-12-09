using AutoMapper;
using BLL.DtoModels;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Repositories.Interfaces;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetPlanQueryHandler : IQueryHandler<GetPlanQuery, PlanModel>
	{
		private readonly IPlanRepository _planRepository;
		private readonly IMapper _mapper;

		public GetPlanQueryHandler(IPlanRepository planRepository, IMapper mapper)
		{
			_planRepository = planRepository;
			_mapper = mapper;
		}

		public async Task<PlanModel> Handle(GetPlanQuery request, CancellationToken cancellationToken)
		{
			List<Plan> plans = await _planRepository
				.GetAsync(p => p.Id == request.PlanId && p.Deleted == false).ToListAsync(cancellationToken);

			if (!plans.Any())
			{
				throw new PlanNotFoundException($"Plan id: {request.PlanId.ToString()}");
			}

			return _mapper.Map<PlanModel>(plans.First());
		}
	}
}