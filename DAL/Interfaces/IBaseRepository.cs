using System.Linq.Expressions;
using DAL.DbModels;
using DAL.Models;

namespace DAL.Interfaces
{
	public interface IBaseRepository<TEntity> where TEntity : BaseEntity
	{
		IEnumerable<TEntity> Get(
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			string includeProperties = "");

		IAsyncEnumerable<TEntity> GetAsync(
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			string includeProperties = "");

		TEntity? GetById(Guid id);

		TEntity? GetById(string id);

		Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);

		bool Any(Expression<Func<TEntity, bool>> filter);

		ValueTask<TEntity?> GetByIdAsync(Guid id);

		Task<TSource> MaxAsync<TSource>(Expression<Func<TEntity, TSource>> selector,
			CancellationToken token = default);

		Task<TSource> MinAsync<TSource>(Expression<Func<TEntity, TSource>> selector,
			CancellationToken token = default);

		//Task<ITableViewResponse<TEntity>> TableView(ITableViewQuery tableQuery, string includeProperties = "");

		//Task<ITableViewResponse<TEntity>> TableView(Expression<Func<TEntity, bool>> filter, ITableViewQuery tableQuery, string includeProperties = "");

		BaseEntityEntry<TEntity> Insert(TEntity entity);

		ValueTask<BaseEntityEntry<TEntity>> InsertAsync(TEntity entity);

		//Task InsertBulkAsync(IEnumerable<TEntity> entities);

		void Delete(Guid id, bool saveChanges);

		Task DeleteAsync(Guid id, bool saveChanges);

		void Delete(TEntity entityToDelete, bool saveChanges);

		Task DeleteAsync(TEntity entityToDelete, bool saveChanges);

		void Update(TEntity entityToUpdate);

		Task SaveChangesAsync(bool acceptAllChangesOnSuccess,
			CancellationToken cancellationToken = default);

		void SaveChanges(bool acceptAllChangesOnSuccess);
	}
}
