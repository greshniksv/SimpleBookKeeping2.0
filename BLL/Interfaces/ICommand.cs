using MediatR;

namespace BLL.Interfaces
{
    public interface ICommand<T> : IRequest<T>
    {
    }
}
