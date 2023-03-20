using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace DAL.Interfaces
{
	public interface IDbContext
	{
		DbContext GetDbContext();

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

		ChangeTracker ChangeTracker { get; }
	}
}
