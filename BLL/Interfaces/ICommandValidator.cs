using MediatR.Pipeline;

namespace BLL.Interfaces
{
    public interface ICommandValidator<TModel, TResponce> : IRequestPreProcessor<TModel>
    where TModel : ICommand<TResponce>
    {
    }
}
