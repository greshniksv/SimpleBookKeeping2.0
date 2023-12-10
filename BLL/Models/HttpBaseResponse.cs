using System.Text.Json.Serialization;
using BLL.Models.Interfaces;
using Newtonsoft.Json;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace BLL.Models
{
	public class HttpBaseResponse<TData> : IValidationError, ICommonError, ICommonReturn<TData>
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public TData Result { get; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IReadOnlyList<ValidationErrorModel> Validation { get; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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
