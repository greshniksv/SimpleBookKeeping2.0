using System.ComponentModel.DataAnnotations;

namespace BLL.DtoModels
{
    public class SpendModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public CostDetailModel CostDetail { get; set; }

        [Range(0, 50000, ErrorMessage = "Расход может быть от 0 до 50 000")]
        public int Value { get; set; }

        public string Comment { get; set; }

        public string Image { get; set; }
    }
}