using MediatR;
using SimpleBookKeeping.Models;

namespace SimpleBookKeeping.Commands.Clear
{
    public class ClearDatabaseCommand : IRequest<bool>
    {
    }
}