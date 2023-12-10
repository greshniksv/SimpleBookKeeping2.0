namespace BLL.Models.Interfaces
{
	public interface ICommonReturn<TData>
	{
		TData Result { get; }
	}
}
