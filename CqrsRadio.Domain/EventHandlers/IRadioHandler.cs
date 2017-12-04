using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    interface IRadioHandler<in TEvent> :
        IHandler<RadioCreated>,
        IHandler<RadioDeleted> where TEvent : IDomainEvent
    { }
}
