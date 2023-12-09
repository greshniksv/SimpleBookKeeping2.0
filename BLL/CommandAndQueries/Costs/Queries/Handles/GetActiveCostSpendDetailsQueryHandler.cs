using AutoMapper;
using BLL.CommandAndQueries.Plans.Queries;
using BLL.DtoModels;
using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BLL.CommandAndQueries.Costs.Queries.Handles
{
	public class GetActiveCostSpendDetailsQueryHandler
		: IQueryHandler<GetActiveCostSpendDetailsQuery, IList<CostSpendDetailModel>>
	{
		private readonly IMediator _mediator;
		private readonly ICostRepository _costRepository;
		private readonly IMapper _mapper;
		private Dictionary<Guid, ApplicationUser> _userCache;
		private readonly UserManager<ApplicationUser> _userManager;

		public GetActiveCostSpendDetailsQueryHandler(IMediator mediator, ICostRepository costRepository,
			IMapper mapper, UserManager<ApplicationUser> userManager)
		{
			_mediator = mediator;
			_costRepository = costRepository;
			_mapper = mapper;
			_userManager = userManager;
		}


		///// <summary>Handles a request</summary>
		///// <param name="message">The request message</param>
		///// <returns>Response from the request</returns>
		//public IList<CostSpendDetailModel> Handle(GetActiveCostSpendDetailsQuery message)
		//{
		//	IList<CostSpendDetailModel> costSpendDetailModels = new List<CostSpendDetailModel>();

		//	var activePlans = _mediator.Send(new GetPlansQuery { IsActive = true, UserId = message.UserId });

		//	using (var session = Db.Session)
		//	{
		//		_userCache = session.QueryOver<User>().List().ToDictionary(x => x.Id, x => x);

		//		foreach (var activePlan in activePlans)
		//		{
		//			var costs = session.QueryOver<Cost>().Where(x => x.Plan.Id == activePlan.Id && x.Deleted == false).List();

		//			if (message.CostId != Guid.Empty)
		//				costs = costs.Where(x => x.Id == message.CostId).ToList();

		//			foreach (var cost in costs)
		//				foreach (var costDetail in cost.CostDetails.Where(x => x.Deleted == false))
		//				{
		//					var item = new CostSpendDetailModel {
		//						CostId = cost.Id,
		//						CostName = cost.Name,
		//						DetailId = costDetail.Id,
		//						Date = costDetail.Date,
		//						Value = costDetail.Value,
		//						Spends = new List<SpendModel>()
		//					};

		//					var spendModels = AutoMapperConfig.Mapper.Map<IList<SpendModel>>(costDetail.Spends.OrderBy(x => x.OrderId));

		//					// Replace other user comment
		//					//foreach (var spendModel in spendModels)
		//					//{
		//					//    if (spendModel.UserId != message.UserId)
		//					//    {
		//					//        spendModel.Comment = GetUser(spendModel.UserId).Name;
		//					//    }
		//					//}

		//					((List<SpendModel>)item.Spends).AddRange(spendModels);

		//					costSpendDetailModels.Add(item);
		//				}
		//		}
		//	}

		//	return costSpendDetailModels;
		//}

		//private User GetUser(Guid userId)
		//{
		//	User user;
		//	if (!_userCache.TryGetValue(userId, out user))
		//		throw new Exception("User not found!");
		//	return user;
		//}

		public async Task<IList<CostSpendDetailModel>> Handle(GetActiveCostSpendDetailsQuery request,
			CancellationToken cancellationToken)
		{
			IList<CostSpendDetailModel> costSpendDetailModels = new List<CostSpendDetailModel>();

			var activePlans = await _mediator.Send(new GetPlansQuery { IsActive = true, UserId = request.UserId },
				cancellationToken);

			_userCache = _userManager.Users.ToList().ToDictionary(x => x.Id, x => x);

			foreach (var activePlan in activePlans)
			{
				var costs = await _costRepository.GetAsync(x =>
					x.Plan.Id == activePlan.Id && x.Deleted == false).ToListAsync(cancellationToken);

				if (request.CostId != Guid.Empty)
				{
					costs = costs.Where(x => x.Id == request.CostId).ToList();
				}

				foreach (var cost in costs)
				{
					foreach (var costDetail in cost.CostDetails.Where(x => x.Deleted == false))
					{
						var item = new CostSpendDetailModel {
							CostId = cost.Id,
							CostName = cost.Name,
							DetailId = costDetail.Id,
							Date = costDetail.Date,
							Value = costDetail.Value,
							Spends = new List<SpendModel>()
						};

						IList<SpendModel> spendModels =
							_mapper.Map<IList<SpendModel>>(costDetail.Spends.OrderBy(x => x.OrderId));
						((List<SpendModel>)item.Spends).AddRange(spendModels);

						costSpendDetailModels.Add(item);
					}
				}
			}

			return costSpendDetailModels;
		}
	}
}