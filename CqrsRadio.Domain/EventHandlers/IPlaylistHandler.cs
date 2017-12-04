using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    public interface IPlaylistHandler<in TEvent> : 
        IHandler<PlaylistAdded>, 
        IHandler<PlaylistCreated>,
        IHandler<PlaylistDeleted> where TEvent : IDomainEvent
    { }
}
