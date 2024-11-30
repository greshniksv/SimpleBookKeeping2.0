using BLL.DtoModels;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.CommandAndQueries.Spends.Commands.Handlers
{
	public class InsertSpendCommandHandler : ICommandHandler<InsertSpendCommand, bool>
	{
		private readonly IMainContext _mainContext;
		private readonly ICostDetailRepository _costDetailRepository;
		private readonly UserManager<ApplicationUser> _userRepository;
		private readonly ISpendRepository _spendRepository;

		public InsertSpendCommandHandler(IMainContext mainContext, ICostDetailRepository costDetailRepository,
			UserManager<ApplicationUser> userRepository, ISpendRepository spendRepository)
		{
			_mainContext = mainContext;
			_costDetailRepository = costDetailRepository;
			_userRepository = userRepository;
			_spendRepository = spendRepository;
		}

		/// <summary>Handles a void request</summary>
		/// <param name="request">The request request</param>
		/// <param name="cancellationToken"></param>
		public async Task<bool> Handle(InsertSpendCommand request, CancellationToken cancellationToken)
		{
			await using IDbContextTransaction transaction = await _mainContext.BeginTransactionAsync(cancellationToken);

			try
			{
				var spendModel = request.SpendModel;
				CostDetail? costDetail = await _costDetailRepository
					.GetAsync(x => x.Id == spendModel.CostDetailId,
						includeProperties: nameof(CostDetail.Spends)).FirstAsync(cancellationToken);

				// New Spend
				Spend spend = new() {
					UserId = ((await _userRepository.FindByIdAsync(request.UserId.ToString()))!).Id,
					Comment = spendModel.Comment,
					CostDetail = costDetail,
					Value = spendModel.Value,
					OrderId = costDetail.Spends.Count,
					Image = spendModel.Image
				};

				await _spendRepository.InsertAsync(spend);
				await _spendRepository.SaveChangesAsync(true, cancellationToken);
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