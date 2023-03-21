using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL.DbContexts
{
	public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>,
		IPersistedGrantDbContext, IConfigurationDbContext, IIdentityContext
	{
		public IdentityContext(DbContextOptions<IdentityContext> options)
			: base(options)
		{
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		}

		public DbSet<PersistedGrant> PersistedGrants { get; set; }
		public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
		public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
		public DbSet<IdentityResource> IdentityResources { get; set; }
		public DbSet<ApiResource> ApiResources { get; set; }
		public DbSet<ApiScope> ApiScopes { get; set; }
		public DbSet<Client> Clients { get; set; }

		public DbContext GetDbContext()
		{
			return this;
		}

		public DatabaseFacade GetDatabase()
		{
			return Database;
		}

		public IModel GetModel()
		{
			return Model;
		}

		public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
		{
			return GetDatabase().BeginTransactionAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var schema = "ids";
			base.OnModelCreating(builder);
			builder.HasPostgresExtension("uuid-ossp");
			builder.HasDefaultSchema(schema);

			Guid adminRoleId = Guid.Parse("b2fb3f71-267f-4d8a-b8f9-3eec3f10899d");
			Guid adminUserId = Guid.Parse("bea54459-3abb-4150-bfd8-ba68c6d5870c");

			var roleId = adminRoleId;
			var userId = adminUserId;

			var role = new ApplicationRole("administrator")
			{
				Id = roleId,
				Name = "administrator",
				NormalizedName = "administrator".ToUpperInvariant(),
				ConcurrencyStamp = null
			};

			var user = new ApplicationUser()
			{
				Id = userId,
				UserName = "admin",
				NormalizedUserName = "ADMIN",
				PasswordHash = "AQAAAAIAAYagAAAAEAxwgNqYtpwlA4yyzI8Qr5np8nQvqI3/vm9lZ2iVdPCwhbqYd2+T9B1Gvy8vaRUp1w==",
				SecurityStamp = "QIG77W2IDH3QVJD3K7JKTNW5VIMX7YRE",
				PhoneNumber = "+710102223344",
				Email = "admin@mail.com",
				ConcurrencyStamp = null
			};

			var userRoles = new List<IdentityUserRole<Guid>>() { new() { UserId = user.Id, RoleId = role.Id } };

			//configuring identity server schema
			builder.Entity<IdentityUserLogin<Guid>>().ToTable("IdentityUserLogins", schema);
			builder.Entity<IdentityUserClaim<Guid>>().ToTable("IdentityUserClaims", schema);
			builder.Entity<IdentityUserToken<Guid>>().ToTable("IdentityUserTokens", schema);
			builder.Entity<ApplicationUser>().ToTable("IdentityUsers", schema).HasData(user);

			builder.Entity<IdentityUserRole<Guid>>().ToTable("IdentityUserRoles", schema)
				.HasData(userRoles);
			builder.Entity<ApplicationRole>().ToTable("IdentityRoles", schema).HasData(role);
			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("IdentityRoleClaims", schema);

			builder.Entity<ClientIdPRestriction>().ToTable("ClientIdPRestrictions", schema);
			builder.Entity<ClientClaim>().ToTable("ClientClaims", schema);
			builder.Entity<ClientScope>().ToTable("ClientScopes", schema);
			builder.Entity<ClientPostLogoutRedirectUri>()
				.ToTable("ClientPostLogoutRedirectUris", schema);
			//builder.Entity<IdentityClient>().ToTable("Clients", schema);
			builder.Entity<ClientCorsOrigin>().ToTable("ClientCorsOrigins", schema);
			builder.Entity<ClientRedirectUri>().ToTable("ClientRedirectUri", schema);
			builder.Entity<ClientGrantType>().ToTable("ClientGrantTypes", schema);
			builder.Entity<ClientSecret>().ToTable("ClientSecrets", schema);

			builder.Entity<IdentityResourceClaim>().ToTable("IdentityResourceClaims", schema);
			builder.Entity<IdentityResource>().ToTable("IdentityResources", schema);

			builder.Entity<IdentityResourceClaim>().ToTable("IdentityResourceClaims", schema);
			builder.Entity<IdentityResource>().ToTable("IdentityResources", schema);

			builder.Entity<ApiScopeClaim>().ToTable("ApiScopeClaims", schema);
			builder.Entity<ApiScope>().ToTable("ApiScopes", schema);
			builder.Entity<ApiResource>().ToTable("ApiResources", schema);
			builder.Entity<ApiResourceSecret>().ToTable("ApiResourceSecrets", schema);
			builder.Entity<ApiResourceClaim>().ToTable("ApiResourceClaims", schema);

			builder.Entity<DeviceFlowCodes>().ToTable("DeviceFlowCodes", schema).HasNoKey();
			builder.Entity<PersistedGrant>().ToTable("PersistedGrants", schema).HasKey(x => x.Key);
		}

		public async Task<int> SaveChangesAsync()
		{
			return await base.SaveChangesAsync(CancellationToken.None);
		}
	}
}
