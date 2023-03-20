using MediatR;

namespace BLL.Interfaces
{
    public interface IQuery<T> : IRequest<T>
    {
    }
}
