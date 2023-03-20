using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BLL.Models
{
	public class HttpBaseResponse<TData>
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public TData Result { get; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public IReadOnlyList<ValidationErrorModel> Validation { get; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public IReadOnlyList<ErrorModel> Errors { get; }

		public HttpBaseResponse(TData data)
		{
			Result = data;
		}

		public HttpBaseResponse(ErrorModel error)
		{
			Errors = new List<ErrorModel>()
			{
				error
			};
		}

		public HttpBaseResponse(IEnumerable<ErrorModel> errors)
		{
			Errors = new List<ErrorModel>(errors);
		}

		public HttpBaseResponse(IEnumerable<ValidationErrorModel> validationErrors)
		{
			Validation = new List<ValidationErrorModel>(validationErrors);
		}
	}
}
