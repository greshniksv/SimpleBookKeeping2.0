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

			CostStatusModel[] list = _mainContext.CostList(plan.Id);
			int allSpends = _mainContext.SpendsSumByPlan(plan.Id);

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
	}
}