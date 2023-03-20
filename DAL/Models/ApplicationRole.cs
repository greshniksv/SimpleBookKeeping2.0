using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
	public class ApplicationRole : IdentityRole<Guid>
	{
		public ApplicationRole()
		{
		}

		public ApplicationRole(string roleName)
			: base(roleName)
		{
		}
	}
}
