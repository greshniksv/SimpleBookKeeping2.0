using BLL.DtoModels;
using DAL.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Interfaces
{
	public interface IMainContext: IDisposable
	{
		DbContext GetDbContext();

		List<CostStatusModel> CostList(Guid planId);

		int SpendsSumByPlan(Guid planId);

		DbSet<User> Users { get; set; }
		DbSet<Spend> Spends { get; set; }
		DbSet<PlanMember> PlanMembers { get; set; }
		DbSet<Plan> Plans { get; set; }
		DbSet<CostDetail> CostDetails { get; set; }
		DbSet<Cost> Costs { get; set; }

		int SaveChanges();

		int SaveChanges(bool acceptAllChangesOnSuccess);

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		Task<int> SaveChangesAsync(
			bool acceptAllChangesOnSuccess,
			CancellationToken cancellationToken = default);

		DbSet<TEntity> Set<TEntity>()
			where TEntity : class;

		DbSet<TEntity> Set<TEntity>(string name)
			where TEntity : class;

		EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
			where TEntity : class;

		EntityEntry Entry(object entity);

		DatabaseFacade GetDatabase();

		IModel GetModel();

		Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
	}
}
