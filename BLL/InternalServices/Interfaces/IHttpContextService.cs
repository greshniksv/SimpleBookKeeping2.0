using System.Security.Claims;
using BLL.Interfaces;

namespace BLL.InternalServices.Interfaces
{
	public interface IHttpContextService : IInternalService
	{
		Guid GetCurrentUserId();

		List<Claim> GetCurrentUserClaims();

		bool IsUserAuthorized();
	}
}
