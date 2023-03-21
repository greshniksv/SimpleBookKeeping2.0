using System;
using System.Collections.Generic;
using MediatR;
using SimpleBookKeeping.Database;
using SimpleBookKeeping.Database.Entities;
using SimpleBookKeeping.Unility;

namespace SimpleBookKeeping.Commands.Clear
{
    public class ClearDatabaseCommandHandler : IRequestHandler<ClearDatabaseCommand, bool>
    {
        public bool Handle(ClearDatabaseCommand message)
        {
            List<Plan> planToRemove = new List<Plan>();
            List<PlanMember> planMemberToRemove = new List<PlanMember>();
            List<Cost> costToRemove = new List<Cost>();
            List<CostDetail> costDetailToRemove = new List<CostDetail>();
            List<Spend> spendToRemove = new List<Spend>();

            using (var session = Db.Session)
            {
                var plans = session.QueryOver<Plan>().Where(x => x.Deleted).List();

                foreach (var plan in plans)
                {
                    planToRemove.Add(plan);
                }

                foreach (var plan in plans)
                {
                    foreach (var planPlanMember in plan.PlanMembers)
                    {
                        planMemberToRemove.Add(planPlanMember);
                    }
                }

                foreach (var plan in plans)
                {
                    foreach (var cost in plan.Costs)
                    {
                        costToRemove.Add(cost);
                        foreach (var costDetail in cost.CostDetails)
                        {
                            costDetailToRemove.Add(costDetail);
                            foreach (var spend in costDetail.Spends)
                            {
                                spendToRemove.Add(spend);
                            }
                        }
                    }
                }

            }


            using (var session = Db.Session)
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    ImageStorage storage = new ImageStorage();

                    foreach (var spend in spendToRemove)
                    {
                        if (!string.IsNullOrEmpty(spend.Image))
                        {
                            storage.Delete(spend.Image);
                        }
                        session.Delete(spend);
                    }

                    foreach (var costDetail in costDetailToRemove)
                    {
                        session.Delete(costDetail);
                    }

                    foreach (var cost in costToRemove)
                    {
                        session.Delete(cost);
                    }

                    foreach (var planMember in planMemberToRemove)
                    {
                        session.Delete(planMember);
                    }

                    foreach (var plan in planToRemove)
                    {
                        session.Delete(plan);
                    }

                    transaction.Commit();
                }
                catch (Exception )
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return true;
        }
    }
}