using AutoMapper;
using BLL.DtoModels;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.CommandAndQueries.Plans.Queries.Handlers
{
	public class GetPlanStatusQueryHandler : IQueryHandler<GetPlanStatusQuery, PlanStatusModel>
	{
		private readonly IPlanRepository _planRepository;
		private readonly IMainContext _mainContext;
		private readonly IMapper _mapper;

		public GetPlanStatusQueryHandler(IPlanRepository planRepository, IMapper mapper, IMainContext mainContext)
		{
			_planRepository = planRepository;
			_mapper = mapper;
			_mainContext = mainContext;
		}

		/// <summary>Handles a request</summary>
		/// <param name="request">The request request</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Response from the request</returns>
		public async Task<PlanStatusModel> Handle(GetPlanStatusQuery request, CancellationToken cancellationToken)
		{
			PlanStatusModel planStatusModel = new PlanStatusModel();
			List<CostStatusModel> costStatusModels = new List<CostStatusModel>();
			Plan plan = await _planRepository.GetAsync(x => x.Id == request.PlanId).FirstAsync(cancellationToken);
			if (plan == null)
			{
				throw new Exception("GetPlanStatusQuery. Plan not found.");
			}
			//var costs = plan.Costs.Where(x => x.Deleted == false).ToList();

			int passedDays = (DateTime.Now.Date - plan.Start.Date).Days;
			int totalDays = (plan.End.Date - plan.Start.Date).Days;

			planStatusModel.Id = plan.Id;
			planStatusModel.Name = plan.Name;
			planStatusModel.Progress = passedDays * 100 / totalDays;



			IReadOnlyList<CostStatusModel> list = CostList(plan.Id);
			int allSpends = SpendsSumByPlan(plan.Id);

			//var list = session.CreateSQLQuery($"exec dbo.CostList @Plan='{plan.Id}'")
			//	.SetResultTransformer(Transformers.AliasToBean<CostStatusModel>()).List<CostStatusModel>();
			//var allSpends = session.CreateSQLQuery($"exec [dbo].[SpendsSumByPlan] @Plan='{plan.Id}'")
			//	.AddScalar("Sum", NHibernateUtil.Int32).List<int>();

			costStatusModels.AddRange(list.OrderBy(x => x.Name).ToList());

			// Balance on start minus sum of planed costs
			planStatusModel.Rest = plan.Balance - allSpends;
			planStatusModel.Balance = costStatusModels.Sum(x => x.Balance);

			planStatusModel.CostStatusModels = costStatusModels;
			return planStatusModel;
		}


		public IReadOnlyList<CostStatusModel> CostList(Guid planId)
		{
			List<CostStatusModel> list = new();
			foreach (Cost cost in _mainContext.Costs.Where(x => x.PlanId == planId && x.Deleted == false).ToList())
			{
				var items = from det in _mainContext.CostDetails
					where det.CostId == cost.Id && det.Deleted == false && det.Date <= DateTime.Now
					select new {
						det,
						spend = (_mainContext.Spends.Where(x => x.CostDetailId == det.Id).Sum(x => x.Value ?? 0)),
						balabse = det.Value - (_mainContext.Spends.Where(x => x.CostDetailId == det.Id).Sum(x => x.Value ?? 0))
					};

				var balance = items.Sum(x => x.balabse);
				list.Add(new CostStatusModel() { Id = cost.Id, Name = cost.Name, Balance = balance });
			}

			return list.AsReadOnly();
		}

		public int SpendsSumByPlan(Guid planId)
		{
			return (from cost in _mainContext.Costs
				join cd in _mainContext.CostDetails on cost.Id equals cd.CostId
				join spend in _mainContext.Spends on cd.Id equals spend.CostDetailId
				where cost.PlanId == planId && cost.Deleted == false && cd.Deleted == false
				select spend.Value ?? 0).Sum();
		}
	}
}