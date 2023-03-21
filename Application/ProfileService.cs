using DAL.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Application
{
	public class ProfileService : IProfileService
	{
		private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly Serilog.ILogger _logger;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IConfiguration _configuration;

		public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
			UserManager<ApplicationUser> userManager, Serilog.ILogger logger, RoleManager<ApplicationRole> roleManager,
			IConfiguration configuration)
		{
			_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
			_userManager = userManager;
			_logger = logger;
			_roleManager = roleManager;
			_configuration = configuration;
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			string sub = context.Subject.GetSubjectId();
			ApplicationUser user = await _userManager.FindByIdAsync(sub);
			ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);
			List<Claim> claims = userClaims.Claims.ToList();

			claims.Add(new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]));
			claims.Add(new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]));

			if (_userManager.SupportsUserRole)
			{
				IList<string> roles = await _userManager.GetRolesAsync(user);
				foreach (var roleName in roles)
				{
					claims.Add(new Claim(JwtClaimTypes.Role, roleName));
					if (_roleManager.SupportsRoleClaims)
					{
						ApplicationRole role = await _roleManager.FindByNameAsync(roleName);
						if (role != null)
						{
							claims.AddRange(await _roleManager.GetClaimsAsync(role));
						}
					}
				}
			}

			claims = claims.Distinct(new DistinctClaimComparer()).ToList();
			context.IssuedClaims.AddRange(claims);
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			ApplicationUser user = await _userManager.GetUserAsync(context.Subject);
			if (user == null)
			{
				_logger.Information($"User not found matching to: {context.Subject.Identity?.Name} ");
			}

			context.IsActive = user != null;
		}

		private class DistinctClaimComparer : IEqualityComparer<Claim>
		{
			public bool Equals(Claim? x, Claim? y)
			{
				if (ReferenceEquals(x, y))
				{
					return true;
				}

				if (ReferenceEquals(x, null))
				{
					return false;
				}

				if (ReferenceEquals(y, null))
				{
					return false;
				}

				if (x.GetType() != y.GetType())
				{
					return false;
				}

				return x.Type == y.Type && x.Value == y.Value;
			}

			public int GetHashCode(Claim obj)
			{
				return HashCode.Combine(obj.Type, obj.Value);
			}
		}
	}
}
