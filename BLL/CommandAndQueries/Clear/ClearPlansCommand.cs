using MediatR;

namespace BLL.CommandAndQueries.Clear
{
    public class ClearDatabaseCommand : IRequest<bool>
    {
    }
}