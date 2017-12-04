using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    public interface IPlaylistHandler : 
        IHandler<PlaylistAdded>, 
        IHandler<PlaylistCreated>,
        IHandler<PlaylistDeleted>
    { }
}
