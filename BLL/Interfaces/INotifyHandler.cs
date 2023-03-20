using MediatR;

namespace BLL.Interfaces
{
	public interface INotifyHandler<TNotify> : IRequestHandler<TNotify, bool>
		where TNotify : INotify
	{
	}
}
