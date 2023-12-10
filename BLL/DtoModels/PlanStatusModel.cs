using DAL.Models;

namespace BLL.DtoModels
{
    public class PlanStatusModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Balance { get; set; }

        public int Rest { get; set; }

        public int Progress { get; set; }

        public int BalanceToEnd { get; set; }

        public IReadOnlyCollection<CostStatusModel> CostStatusModels { get; set; }
    }
}