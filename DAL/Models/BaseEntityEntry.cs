using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
	public class BaseEntityEntry<T>
		where T : class
	{
		private readonly T _entry;
		private readonly EntityState _entityState;
		private readonly EntityEntry<T> _entityEntry;

		public BaseEntityEntry(EntityEntry<T> entityEntry)
		{
			_entityEntry = entityEntry;
		}

		internal BaseEntityEntry(T entry, EntityState entityState)
		{
			_entry = entry;
			_entityState = entityState;
			_entityEntry = null;
		}

		public BaseEntityEntry()
		{
		}

		public T Entity {
			get { return _entityEntry?.Entity ?? _entry; }
		}

		public EntityState EntityState {
			get { return _entityEntry?.State ?? _entityState; }
		}
	}
}
