using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.CommandAndQueries.Spends.Commands.Handlers
{
	public class SaveSpendCommandHandler : ICommandHandler<SaveSpendCommand, bool>
	{
		private readonly IMainContext _mainContext;
		private readonly ICostDetailRepository _costDetailRepository;
		private readonly IUserRepository _userRepository;
		private readonly ISpendRepository _spendRepository;

		public SaveSpendCommandHandler(IMainContext mainContext, ICostDetailRepository costDetailRepository,
			IUserRepository userRepository, ISpendRepository spendRepository)
		{
			_mainContext = mainContext;
			_costDetailRepository = costDetailRepository;
			_userRepository = userRepository;
			_spendRepository = spendRepository;
		}

		/// <summary>Handles a void request</summary>
		/// <param name="request">The request request</param>
		/// <param name="cancellationToken"></param>
		public async Task<bool> Handle(SaveSpendCommand request, CancellationToken cancellationToken)
		{
			if (!request.SpendModels.Any())
			{
				return true;
			}

			await using IDbContextTransaction transaction = await _mainContext.BeginTransactionAsync(cancellationToken);

			try
			{
				foreach (var spendModel in request.SpendModels)
				{
					if (spendModel.Id == null || spendModel.Id == Guid.Empty)
					{
						CostDetail? costDetail = await _costDetailRepository.GetByIdAsync(spendModel.CostDetailId);

						// New Spend
						Spend spend = new Spend {
							User = await _userRepository.GetByIdAsync(request.UserId),
							Comment = spendModel.Comment,
							CostDetail = costDetail,
							Value = spendModel.Value,
							OrderId = costDetail.Spends.Count,
							Image = spendModel.Image
						};

						await _spendRepository.InsertAsync(spend);
						await _spendRepository.SaveChangesAsync(true, cancellationToken);
					}
					else
					{
						Spend oldSpend = await _spendRepository.GetByIdAsync(spendModel.Id.Value);

						if (spendModel.Value == 0 && spendModel.Comment == null)
						{
							//if (!string.IsNullOrEmpty(oldSpend.Image))
							//{
							//	ImageStorage storage = new ImageStorage();
							//	storage.Delete(oldSpend.Image);
							//}

							// Remove Spend
							await _spendRepository.DeleteAsync(oldSpend, true);
						}
						else
						{
							// Update Spend
							oldSpend.User = await _userRepository.GetByIdAsync(request.UserId);
							oldSpend.Comment = spendModel.Comment;
							oldSpend.CostDetail = await _costDetailRepository.GetByIdAsync(spendModel.CostDetailId);
							oldSpend.Value = spendModel.Value;

							_spendRepository.Update(oldSpend);
							await _spendRepository.SaveChangesAsync(true, cancellationToken);
						}
					}
				}

				await transaction.CommitAsync(cancellationToken);
			}
			catch (Exception e)
			{
				await transaction.RollbackAsync(cancellationToken);
			}

			return true;
		}
	}
}