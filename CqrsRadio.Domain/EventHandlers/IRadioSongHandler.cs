using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    public interface IRadioSongHandler :
        IHandler<RadioSongParsed>,
        IHandler<RadioSongDuplicate>,
        IHandler<RadioSongError>
    {
    }
}
