using IdentityServer4;
using IdentityServer4.Models;
using ApiScope = IdentityServer4.Models.ApiScope;
using Client = IdentityServer4.Models.Client;

namespace Application
{
	public static class Config
	{
		public static IEnumerable<ApiScope> ApiScopes =>
			new List<ApiScope>
			{
				new("openid", "OpenID")
			};

		public static IReadOnlyList<Client> GetClients()
		{
			return (new List<Client> {
				new() {
					ClientId = "client",
					AllowedGrantTypes = { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
					RequireClientSecret = false,
					AccessTokenType = AccessTokenType.Jwt,
					AllowOfflineAccess = true,
					RefreshTokenUsage = TokenUsage.OneTimeOnly,
					AllowAccessTokensViaBrowser = true,

					// scopes that client has access to
					AllowedScopes = {
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.OfflineAccess,
						IdentityServerConstants.StandardScopes.Profile
					}
				}
			}).AsReadOnly();
		}
	}
}
