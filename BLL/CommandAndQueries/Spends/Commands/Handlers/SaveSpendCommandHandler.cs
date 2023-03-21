using System;
using System.Linq;
using BLL.CommandAndQueries.Spends.Commands;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Unility;

namespace BLL.CommandAndQueries.Spends.Commands.Handlers
{
	public class SaveSpendCommandHandler : RequestHandler<SaveSpendCommand>
	{
		/// <summary>Handles a void request</summary>
		/// <param name="message">The request message</param>
		protected override void HandleCore(SaveSpendCommand message)
		{
			if (!message.SpendModels.Any())
				return;

			using (var session = Db.Session)
			using (var transaction = session.BeginTransaction())
			{
				foreach (var spendModel in message.SpendModels)
					if (spendModel.Id == null || spendModel.Id == Guid.Empty)
					{
						var costDetail =
							session.QueryOver<CostDetail>().Where(x => x.Id == spendModel.CostDetailId).List().First();

						// New Spend
						Spend spend = new Spend {
							User = session.QueryOver<User>().Where(x => x.Id == message.UserId).List().First(),
							Comment = spendModel.Comment,
							CostDetail = session.QueryOver<CostDetail>().Where(x => x.Id == spendModel.CostDetailId).List().First(),
							Value = spendModel.Value,
							OrderId = costDetail.Spends.Count,
							Image = spendModel.Image
						};

						session.Save(spend);
					}
					else
					{
						Spend oldSpend = session.QueryOver<Spend>().Where(x => x.Id == spendModel.Id).List().First();

						if (spendModel.Value == 0 && spendModel.Comment == null)
						{
							if (!string.IsNullOrEmpty(oldSpend.Image))
							{
								ImageStorage storage = new ImageStorage();
								storage.Delete(oldSpend.Image);
							}

							// Remove Spend
							session.Delete(oldSpend);
						}
						else
						{
							// Update Spend
							oldSpend.User = session.QueryOver<User>().Where(x => x.Id == message.UserId).List().First();
							oldSpend.Comment = spendModel.Comment;
							oldSpend.CostDetail = session.QueryOver<CostDetail>().Where(x => x.Id == spendModel.CostDetailId).List().First();
							oldSpend.Value = spendModel.Value;

							session.SaveOrUpdate(oldSpend);
						}
					}
				transaction.Commit();
			}
		}
	}
}