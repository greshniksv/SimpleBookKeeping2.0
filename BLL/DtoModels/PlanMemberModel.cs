namespace BLL.DtoModels
{
    public class PlanMemberModel
    {
        public Guid Id { get; set; }

        public UserModel User { get; set; }

        public PlanModel Plan { get; set; }
    }
}