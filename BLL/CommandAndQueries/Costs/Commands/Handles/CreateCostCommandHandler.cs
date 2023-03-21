using BLL.DtoModels;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Repositories.Interfaces;
using SimpleBookKeeping.Exceptions;

namespace BLL.CommandAndQueries.Costs.Commands.Handles
{
	public class CreateCostCommandHandler : ICommandHandler<CreateCostCommand, CostModel>
	{
		private readonly IPlanRepository _planRepository;

		public CreateCostCommandHandler(IPlanRepository planRepository)
		{
			_planRepository = planRepository;
		}

		public async Task<CostModel> Handle(CreateCostCommand request, CancellationToken cancellationToken)
		{
			Plan plan = ( await _planRepository.GetAsync(p => p.Id == request.PlanId)).FirstOrDefault();

			if (plan == null)
			{
				throw new PlanNotFoundException($"Plan id: {request.PlanId}");
			}

			var costDetails = new List<CostDetailModel>();

			for (DateTime i = plan.Start; i < plan.End; i = i.AddDays(1))
			{
				costDetails.Add(new CostDetailModel { Date = i, Value = 0 });
			}

			var model = new CostModel {
				Name = string.Empty,
				CostDetails = costDetails,
				PlanId = request.PlanId
			};

			return model;
		}
	}
}