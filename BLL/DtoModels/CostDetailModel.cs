using System.ComponentModel.DataAnnotations;

namespace BLL.DtoModels
{
    public class CostDetailModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Дата должна быть указана")]
        public DateTime Date { get; set; }

        [Range(0, 5000000, ErrorMessage = "Расход может быть от 0 до 5 000 000")]
        public int? Value { get; set; }
    }
}