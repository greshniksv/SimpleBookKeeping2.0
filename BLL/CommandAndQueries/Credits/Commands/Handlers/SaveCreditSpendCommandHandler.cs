using System;
using System.Collections.Generic;
using System.Linq;
using BLL.CommandAndQueries.Credits.Commands;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;

namespace BLL.CommandAndQueries.Credits.Commands.Handlers
{
	public class SaveCreditSpendCommandHandler : RequestHandler<SaveCreditSpendCommand>
	{
		/// <summary>Handles a void request</summary>
		/// <param name="message">The request message</param>
		protected override void HandleCore(SaveCreditSpendCommand message)
		{
			if (!message.SpendModels.Any())
				return;

			using (var session = Db.Session)
			using (var transaction = session.BeginTransaction())
			{
				foreach (var spendModel in message.SpendModels)
				{
					var partOfSumm = spendModel.Value / spendModel.Days;
					var days = spendModel.Days;
					var selectedCostDetail = new List<CostDetail>();

					var costDetailList =
						session.QueryOver<CostDetail>()
							.Where(x => x.Cost.Id == spendModel.CostId)
							.List()
							.OrderBy(x => x.Date);

					bool startSelect = false;
					foreach (var costDetail in costDetailList)
					{
						if (startSelect)
							selectedCostDetail.Add(costDetail);

						if (costDetail.Id == spendModel.CostDetailId)
						{
							startSelect = true;
							selectedCostDetail.Add(costDetail);
						}

						if (selectedCostDetail.Count >= days)
							break;
					}

					foreach (var costDetailItem in selectedCostDetail)

						if (spendModel.Id == null || spendModel.Id == Guid.Empty)
						{

							// New Spend
							Spend spend = new Spend {
								User = session.QueryOver<User>().Where(x => x.Id == message.UserId).List().First(),
								Comment = spendModel.Comment,
								CostDetail = session.QueryOver<CostDetail>().Where(x => x.Id == costDetailItem.Id).List().First(),
								Value = partOfSumm,
								OrderId = costDetailItem.Spends.Count
							};

							session.Save(spend);
						}
						else
						{
							Spend oldSpend = session.QueryOver<Spend>().Where(x => x.Id == spendModel.Id).List().First();

							if (spendModel.Value == 0 && spendModel.Comment == null)
								// Remove Spend
								session.Delete(oldSpend);
							else
							{
								// Update Spend
								oldSpend.User = session.QueryOver<User>().Where(x => x.Id == message.UserId).List().First();
								oldSpend.Comment = spendModel.Comment;
								oldSpend.CostDetail = session.QueryOver<CostDetail>().Where(x => x.Id == costDetailItem.Id).List().First();
								oldSpend.Value = spendModel.Value;

								session.SaveOrUpdate(oldSpend);
							}
						}

				}
				transaction.Commit();
			}
		}
	}
}