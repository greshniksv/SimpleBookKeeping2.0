using BLL.DtoModels;
using DAL.DbModels;
using DAL.Interfaces;
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
		}

		public DbContext GetDbContext()
		{
			return this;
		}

		public List<CostStatusModel> CostList(Guid planId)
		{
			throw new NotSupportedException();
		}

		public int SpendsSumByPlan(Guid planId)
		{
			throw new NotSupportedException();
		}

		public DbSet<User> Users { get; set; }
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
			modelBuilder.HasDbFunction(typeof(MainContext).GetMethod(nameof(CostList),
				new[] { typeof(List<CostStatusModel>) })).HasName("CostList");
			modelBuilder.HasDbFunction(typeof(MainContext).GetMethod(nameof(SpendsSumByPlan),
				new[] { typeof(int) })).HasName("SpendsSumByPlan");
		}
	}
}
