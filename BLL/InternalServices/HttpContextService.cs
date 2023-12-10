using BLL.InternalServices.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BLL.Exceptions;

namespace BLL.InternalServices
{
	public class HttpContextService : IHttpContextService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HttpContextService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public Guid GetCurrentUserId()
		{
			string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!string.IsNullOrEmpty(userId))
			{
				return Guid.Parse(userId);
			}
			else
			{
				throw new HttpContextServiceException("User not found in HttpContext");
			}
		}

		public List<Claim> GetCurrentUserClaims()
		{
			return _httpContextAccessor.HttpContext?.User.Claims.ToList() ?? new List<Claim>();
		}

		public bool IsUserAuthorized()
		{
			return _httpContextAccessor.HttpContext?.User?
				.FindFirstValue(ClaimTypes.NameIdentifier) is not null;
		}
	}
}
