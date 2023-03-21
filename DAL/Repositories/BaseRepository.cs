using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DAL.DbModels;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
	abstract public class BaseRepository<TEntity> : IBaseRepository<TEntity>
		where TEntity : BaseEntity
	{
		internal IMainContext Context;
		internal DbContext DbContext;
		internal DbSet<TEntity> DbSet;

		protected BaseRepository(IMainContext context)
		{
			Context = context;
			DbContext = context.GetDbContext();
			DbSet = context.Set<TEntity>();
		}

		public virtual IEnumerable<TEntity> Get(
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			string includeProperties = "")
		{
			IQueryable<TEntity> query = DbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

			return orderBy != null ? orderBy(query).ToList() : query.ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> GetAsync(
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			string includeProperties = "")
		{
			IQueryable<TEntity> query = DbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			foreach (var includeProperty in includeProperties.Split
						 (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}

			if (orderBy != null)
			{
				return await orderBy(query).ToListAsync();
			}
			else
			{
				return await query.ToListAsync();
			}
		}

		public virtual TEntity? GetById(Guid id)
		{
			return DbSet.Find(id);
		}

		public virtual TEntity? GetById(string id)
		{
			return DbSet.Find(id);
		}

		public virtual bool Any()
		{
			return DbSet.Any();
		}

		public virtual async Task<bool> AnyAsync()
		{
			return await DbSet.AnyAsync();
		}

		public virtual async ValueTask<TEntity?> GetByIdAsync(Guid id)
		{
			return await DbSet.FindAsync(id);
		}

		public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = DbSet;
			return await query.AnyAsync(filter);
		}

		public bool Any(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = DbSet;
			return query.Any(filter);
		}

		public virtual async Task<TSource> MaxAsync<TSource>(Expression<Func<TEntity, TSource>> selector,
			CancellationToken token = default)
		{
			return await DbSet.MaxAsync(selector, token);
		}

		public virtual async Task<TSource> MinAsync<TSource>(Expression<Func<TEntity, TSource>> selector,
			CancellationToken token = default)
		{
			return await DbSet.MinAsync(selector, token);
		}

		//public async virtual Task<ITableViewResponse<TEntity>> TableView(ITableViewQuery tableQuery, string includeProperties)
		//{
		//	IQueryable<TEntity> query = DbSet;
		//	return await TableViewService.ApplyTableViewAsync<TEntity, TKey>(query, tableQuery, includeProperties);
		//}

		//public async virtual Task<ITableViewResponse<TEntity>> TableView(Expression<Func<TEntity, bool>> filter,
		//	ITableViewQuery tableQuery, string includeProperties)
		//{
		//	IQueryable<TEntity> query = DbSet.Where(filter);
		//	return await TableViewService.ApplyTableViewAsync<TEntity, TKey>(query, tableQuery, includeProperties);
		//}

		public virtual BaseEntityEntry<TEntity> Insert(TEntity entity)
		{
			return new BaseEntityEntry<TEntity>(DbSet.Add(entity));
		}

		public virtual async ValueTask<BaseEntityEntry<TEntity>> InsertAsync(TEntity entity)
		{
			return new BaseEntityEntry<TEntity>(await DbSet.AddAsync(entity));
		}

		//public async virtual Task InsertBulkAsync(IEnumerable<TEntity> entities)
		//{
		//	List<TEntity> list = entities.ToList();
		//	foreach (var entity in list)
		//	{
		//		entity.GetType().GetProperty("Id")?.SetValue(entity, Guid.NewGuid());
		//	}

		//	await DbContext.BulkInsertAsync(list);
		//}

		public virtual void Delete(Guid id, bool saveChanges)
		{
			TEntity entityToDelete = DbSet.Find(id);
			if (entityToDelete == null)
			{
				throw new ItemNotFoundException($"Not found '{typeof(TEntity).Name}' by id: '{id.ToString()}'");
			}

			Delete(entityToDelete, saveChanges);
		}

		public virtual async Task DeleteAsync(Guid id, bool saveChanges)
		{
			TEntity entityToDelete = await DbSet.FindAsync(id);
			if (entityToDelete == null)
			{
				throw new ItemNotFoundException($"Not found '{typeof(TEntity).Name}' by id: '{id}'");
			}

			await DeleteAsync(entityToDelete, saveChanges);
		}

		public virtual void Delete(TEntity entityToDelete, bool saveChanges)
		{
			if (Context.Entry(entityToDelete).State == EntityState.Detached)
			{
				DbSet.Attach(entityToDelete);
			}

			DbSet.Remove(entityToDelete);

			if (saveChanges) { SaveChanges(true); }
		}

		public virtual async Task DeleteAsync(TEntity entityToDelete, bool saveChanges)
		{
			if (Context.Entry(entityToDelete).State == EntityState.Detached)
			{
				DbSet.Attach(entityToDelete);
			}

			DbSet.Remove(entityToDelete);

			if (saveChanges) { await SaveChangesAsync(true); }
		}

		public virtual void Update(TEntity entityToUpdate)
		{
			DbSet.Attach(entityToUpdate);
			Context.Entry(entityToUpdate).State = EntityState.Modified;
		}

		public void SaveChanges(bool acceptAllChangesOnSuccess)
		{
			Context.SaveChanges(acceptAllChangesOnSuccess);
		}

		public virtual async Task SaveChangesAsync(bool acceptAllChangesOnSuccess,
			CancellationToken cancellationToken = default)
		{
			await Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
}
