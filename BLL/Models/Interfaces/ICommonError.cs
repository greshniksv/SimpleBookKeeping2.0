
namespace BLL.Models.Interfaces
{
	public interface ICommonError
	{
		IReadOnlyList<ErrorModel> Errors { get; }
	}
}
