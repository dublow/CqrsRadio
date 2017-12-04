using CqrsRadio.Domain.Events;

namespace CqrsRadio.Domain.Handlers
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent;
    }
}
