using AutoMapper;
using BLL.DtoModels;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Repositories.Interfaces;
using SimpleBookKeeping.Exceptions;

namespace BLL.CommandAndQueries.Costs.Queries.Handles
{
	public class GetCostsQueryHandler : IQueryHandler<GetCostsQuery, IList<CostModel>>
	{
		private readonly ICostRepository _costRepository;
		private readonly IMapper _mapper;

		public GetCostsQueryHandler(ICostRepository costRepository, IMapper mapper)
		{
			_costRepository = costRepository;
			_mapper = mapper;
		}

		public async Task<IList<CostModel>> Handle(GetCostsQuery request, CancellationToken cancellationToken)
		{
			if (request.PlanId == Guid.Empty)
			{
				throw new PlanNotFoundException(request.PlanId.ToString());
			}

			IList<Cost> costs;
			if (request.ShowDeleted)
			{
				costs = (await _costRepository.GetAsync(x =>
					x.Plan.Id == request.PlanId)).ToList();
			}
			else
			{
				costs = (await _costRepository.GetAsync(x =>
					x.Plan.Id == request.PlanId && x.Deleted == false)).ToList();
			}

			IList<CostModel> costModels = _mapper.Map<IList<CostModel>>(costs);

			return costModels;
		}
	}
}