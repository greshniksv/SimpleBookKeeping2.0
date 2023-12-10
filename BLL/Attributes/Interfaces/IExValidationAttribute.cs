namespace BLL.Attributes.Interfaces
{
	public interface IExValidationAttribute
	{
		string[] Parameters { get; }

		string? ErrorMessage { get; }
	}
}
