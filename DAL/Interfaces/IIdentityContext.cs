using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Interfaces
{
	public interface IIdentityContext : IDbContext, IDisposable
	{
		DbSet<IdentityUserToken<Guid>> UserTokens { get; set; }

		DbSet<IdentityUserLogin<Guid>> UserLogins { get; set; }

		DbSet<IdentityUserClaim<Guid>> UserClaims { get; set; }

		DbSet<ApplicationUser> Users { get; set; }

		DbSet<IdentityUserRole<Guid>> UserRoles { get; set; }

		DbSet<ApplicationRole> Roles { get; set; }

		DbSet<IdentityRoleClaim<Guid>> RoleClaims { get; set; }
	}
}
