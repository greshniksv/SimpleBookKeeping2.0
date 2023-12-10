namespace BLL.Models.Interfaces
{
	public interface IValidationError
	{
		IReadOnlyList<ValidationErrorModel> Validation { get; }
	}
}
