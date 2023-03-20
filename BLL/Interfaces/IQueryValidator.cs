using MediatR.Pipeline;

namespace BLL.Interfaces
{
	public interface IQueryValidator<TModel, TResponse> : IRequestPreProcessor<TModel>
	where TModel : IQuery<TResponse>
	{
	}
}
