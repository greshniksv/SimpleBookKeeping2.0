using System.ComponentModel.DataAnnotations;

namespace BLL.DtoModels
{
    public class PlanModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Начало должно быть указано")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Завершение должно быть указано")]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "Имя должно быть указано")]
        [StringLength(maximumLength: 30, ErrorMessage = "Имя не должно превышать 10 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Баланс должен быть указан")]
        [Range(1,5000000,ErrorMessage = "Баланс может быть от 1 до 5'000'000")]
        public int Balance { get; set; }

        public List<Guid> UserMembers { get; set; }
    }
}