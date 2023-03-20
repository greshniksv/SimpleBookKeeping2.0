using MediatR.Pipeline;

namespace BLL.Interfaces
{
	public interface INotifyValidator<TNotify> : IRequestPreProcessor<TNotify>
		where TNotify : INotify
	{
	}
}
