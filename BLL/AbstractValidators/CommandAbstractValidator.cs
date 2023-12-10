using BLL.Interfaces;
using FluentValidation;

namespace BLL.AbstractValidators
{
	public abstract class CommandAbstractValidator<TRequest, TResponse> :
		AbstractValidator<TRequest>, ICommandValidator<TRequest, TResponse>
		where TRequest : ICommand<TResponse>
	{
		public async Task Process(TRequest request, CancellationToken cancellationToken)
		{
			await this.ValidateAndThrowAsync(request, cancellationToken);
		}
	}
}
