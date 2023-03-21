using System.ComponentModel.DataAnnotations;

namespace BLL.DtoModels
{
    public class SimpleCostModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Идентификатор плана должен быть указан")]
        public Guid PlanId { get; set; }

        [Required(ErrorMessage = "Имя должно быть указано")]
        [StringLength(maximumLength: 30, ErrorMessage = "Имя не должно превышать 10 символов")]
        public string Name { get; set; }
    }
}