using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.CommandAndQueries.Credits.Commands.Handlers
{
	public class SaveCreditSpendCommandHandler : ICommandHandler<SaveCreditSpendCommand, bool>
	{
		private readonly ICostDetailRepository _costDetailRepository;
		private readonly IUserRepository _userRepository;
		private readonly ISpendRepository _spendRepository;
		private readonly IMainContext _mainContext;

		public SaveCreditSpendCommandHandler(ICostDetailRepository costDetailRepository,
			IUserRepository userRepository, ISpendRepository spendRepository, IMainContext mainContext)
		{
			_costDetailRepository = costDetailRepository;
			_userRepository = userRepository;
			_spendRepository = spendRepository;
			_mainContext = mainContext;
		}

		/// <summary>Handles a void request</summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		public async Task<bool> Handle(SaveCreditSpendCommand request, CancellationToken cancellationToken)
		{
			if (!request.SpendModels.Any())
			{
				return false;
			}

			await using IDbContextTransaction
				transaction = await _mainContext.BeginTransactionAsync(cancellationToken);
			try
			{
				foreach (var spendModel in request.SpendModels)
				{
					int partOfSum = spendModel.Value / spendModel.Days;
					int days = spendModel.Days;
					var selectedCostDetail = new List<CostDetail>();

					IEnumerable<CostDetail> costDetailList =
						await _costDetailRepository
							.GetAsync(
								x => x.Cost.Id == spendModel.CostId,
								x => x.OrderBy(c => c.Date)).ToListAsync(cancellationToken);

					bool startSelect = false;
					foreach (var costDetail in costDetailList)
					{
						if (startSelect)
						{
							selectedCostDetail.Add(costDetail);
						}

						if (costDetail.Id == spendModel.CostDetailId)
						{
							startSelect = true;
							selectedCostDetail.Add(costDetail);
						}

						if (selectedCostDetail.Count >= days)
						{
							break;
						}
					}

					foreach (var costDetailItem in selectedCostDetail)
					{
						if (spendModel.Id == null || spendModel.Id == Guid.Empty)
						{
							// New Spend
							Spend spend = new Spend {
								User = await _userRepository.GetAsync(x =>
									x.Id == request.UserId).FirstAsync(cancellationToken),
								Comment = spendModel.Comment,
								CostDetail = await _costDetailRepository.GetAsync(x =>
									x.Id == costDetailItem.Id).FirstAsync(cancellationToken),
								Value = partOfSum,
								OrderId = costDetailItem.Spends.Count
							};

							await _spendRepository.InsertAsync(spend);
							await _spendRepository.SaveChangesAsync(true, cancellationToken);
						}
						else
						{
							Spend oldSpend = await _spendRepository.GetAsync(x => x.Id == spendModel.Id).FirstAsync(cancellationToken);

							if (spendModel.Value == 0 && spendModel.Comment == null)
							{
								// Remove Spend
								await _spendRepository.DeleteAsync(oldSpend.Id, true);
							}
							else
							{
								// Update Spend
								oldSpend.User = await _userRepository.GetAsync(x =>
									x.Id == request.UserId).FirstAsync(cancellationToken);
								oldSpend.Comment = spendModel.Comment;
								oldSpend.CostDetail = await _costDetailRepository.GetAsync(x =>
									x.Id == costDetailItem.Id).FirstAsync(cancellationToken);
								oldSpend.Value = spendModel.Value;

								_spendRepository.Update(oldSpend);
								await _spendRepository.SaveChangesAsync(true, cancellationToken);
							}
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