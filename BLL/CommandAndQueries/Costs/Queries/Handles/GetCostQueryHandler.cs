﻿using AutoMapper;
using BLL.DtoModels;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Repositories.Interfaces;

namespace BLL.CommandAndQueries.Costs.Queries.Handles
{
	public class GetCostQueryHandler : IQueryHandler<GetCostQuery, CostModel>
	{
		private readonly ICostRepository _costRepository;
		private readonly IMapper _mapper;

		public GetCostQueryHandler(ICostRepository costRepository, IMapper mapper)
		{
			_costRepository = costRepository;
			_mapper = mapper;
		}

		public async Task<CostModel> Handle(GetCostQuery request, CancellationToken cancellationToken)
		{
			Cost? cost = await _costRepository.GetAsync(x => x.Id == request.CostId, null , "CostDetails").FirstAsync(cancellationToken);
			if (cost == null)
			{
				throw new CostNotFoundException(request.CostId.ToString());
			}

			CostModel item = new();
			_mapper.Map(cost, item);

			item.CostDetails = item.CostDetails.OrderBy(x => x.Date).ToList();
			return item;
		}
	}
}