namespace BLL.DtoModels
{
    public class CostSpendDetailModel
    {
        public Guid CostId { get; set; }

        public string CostName { get; set; }

        public Guid DetailId { get; set; }

        public DateTime Date { get; set; }

        public int? Value { get; set; }

        public IList<SpendModel> Spends { get; set; }
    }
}