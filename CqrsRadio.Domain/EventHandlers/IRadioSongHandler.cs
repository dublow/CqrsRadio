using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    public interface IRadioSongHandler<in TEvent> :
        IHandler<RadioSongParsed>,
        IHandler<RadioSongDuplicate>,
        IHandler<RadioSongError>
        where TEvent : IDomainEvent
    {
    }
}
