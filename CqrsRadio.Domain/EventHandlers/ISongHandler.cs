using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    public interface ISongHandler :
        IHandler<SongAdded>
    { }
}
