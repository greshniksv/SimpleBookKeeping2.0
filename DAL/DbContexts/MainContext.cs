using DAL.DbModels;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.DbContexts
{
	public class MainContext : DbContext, IMainContext
	{
		public MainContext(DbContextOptions<MainContext> options)
			: base(options)
		{
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		}

		public DbContext GetDbContext()
		{
			return this;
		}

		public DbSet<Spend> Spends { get; set; }
		public DbSet<PlanMember> PlanMembers { get; set; }
		public DbSet<Plan> Plans { get; set; }
		public DbSet<CostDetail> CostDetails { get; set; }
		public DbSet<Cost> Costs { get; set; }

		public DatabaseFacade GetDatabase()
		{
			return Database;
		}

		public IModel GetModel()
		{
			return Model;
		}

		public Task<IDbContextTransaction> BeginTransactionAsync(
			CancellationToken cancellationToken = default)
		{
			return GetDatabase().BeginTransactionAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasPostgresExtension("uuid-ossp");
			//var mm = typeof(MainContext).GetMethod(nameof(CostList));
			//modelBuilder.HasDbFunction(mm).HasName("CostList");
			//modelBuilder.HasDbFunction(typeof(MainContext).GetMethod(nameof(SpendsSumByPlan))).HasName("SpendsSumByPlan");
		}
	}
}
