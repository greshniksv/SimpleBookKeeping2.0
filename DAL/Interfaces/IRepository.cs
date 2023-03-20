namespace DAL.Interfaces
{
	public interface IRepository<TModel, TResponce>
	{
		public Task<TResponce> ExecuteAsync(TModel parameter);
	}
}
