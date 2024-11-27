using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.CommandAndQueries.Costs.Commands.Handles
{
	public class SaveCostCommandHandler : ICommandHandler<SaveCostCommand, bool>
	{
		private readonly IMapper _mapper;
		private readonly IPlanRepository _planRepository;
		private readonly ICostRepository _costRepository;
		private readonly ICostDetailRepository _costDetailRepository;
		private readonly IMainContext _mainContext;

		public SaveCostCommandHandler(IMapper mapper, IPlanRepository planRepository, ICostRepository costRepository,
			ICostDetailRepository costDetailRepository, IMainContext mainContext)
		{
			_mapper = mapper;
			_planRepository = planRepository;
			_costRepository = costRepository;
			_costDetailRepository = costDetailRepository;
			_mainContext = mainContext;
		}

		public async Task<bool> Handle(SaveCostCommand request, CancellationToken cancellationToken)
		{
			Plan plan = await _planRepository.GetAsync(p => p.Id == request.Cost.PlanId).FirstAsync(cancellationToken);
			if (request.Cost.Id != Guid.Empty)
			{
				var existCost = await _costRepository.GetAsync(x => x.Id == request.Cost.Id).FirstAsync(cancellationToken);
				if (existCost == null)
				{
					throw new CostNotFoundException(request.Cost.Id.ToString());
				}

				await _costRepository.DeleteAsync(existCost.Id, true);
			}

			Cost cost = new() { Plan = plan };
			_mapper.Map(request.Cost, cost);

			await using IDbContextTransaction transaction = await _mainContext.BeginTransactionAsync(cancellationToken);
			try
			{
				await _costRepository.InsertAsync(cost);

				// Insert new details
				foreach (var costDetailModel in request.Cost.CostDetails)
				{
					costDetailModel.Id = Guid.Empty;
					var detail = new CostDetail();
					_mapper.Map(costDetailModel, detail);
					detail.Cost = cost;
					await _costDetailRepository.InsertAsync(detail);
				}

				await _costDetailRepository.SaveChangesAsync(true, cancellationToken);
				await transaction.CommitAsync(cancellationToken);
			}
			catch (Exception e)
			{
				await transaction.RollbackAsync(cancellationToken);
				throw;
			}

			return true;
		}
	}
}