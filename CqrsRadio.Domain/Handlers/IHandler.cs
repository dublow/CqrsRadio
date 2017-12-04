using CqrsRadio.Domain.Events;

namespace CqrsRadio.Domain.Handlers
{
    public interface IHandler
    {

    }
    public interface IHandler<in TEvent> : IHandler where TEvent : IDomainEvent
    {
        void Handle(TEvent evt);
    }
}
