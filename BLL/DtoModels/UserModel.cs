using DAL.DbModels;

namespace BLL.DtoModels
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public IList<Plan> Plans { get; set; }

        public IList<PlanMember> PlanMembers { get; set; }

        public bool EqualId(UserModel model)
        {
            return Id == model.Id;
        }
    }
}