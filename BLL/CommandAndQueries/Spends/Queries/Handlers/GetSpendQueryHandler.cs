using AutoMapper;
using BLL.DtoModels;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Repositories.Interfaces;

namespace BLL.CommandAndQueries.Spends.Queries.Handlers
{
	public class GetSpendQueryHandler : IQueryHandler<GetSpendQuery, SpendModel>
	{
		private readonly ISpendRepository _repository;
		private readonly IMapper _mapper;

		public GetSpendQueryHandler(ISpendRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<SpendModel> Handle(GetSpendQuery request, CancellationToken cancellationToken)
		{
			Spend spend = await _repository.GetByIdAsync(request.SpendId);
			SpendModel spendModel = _mapper.Map<SpendModel>(spend);
			return spendModel;
		}
	}
}