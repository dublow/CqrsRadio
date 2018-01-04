using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Infrastructure.Bus
{
    public class EventBus : IEventPublisher
    {
        private readonly IEventStream _history;
        private readonly List<IHandler> _subscribers;

        public EventBus(IEventStream history)
        {
            _history = history;
            _subscribers = new List<IHandler>();
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : IDomainEvent
        {
            _history.Add(evt);

            foreach (var subscriber in _subscribers.OfType<IHandler<TEvent>>())
            {
                subscriber.Handle(evt);
            }
        }

        public void Subscribe(IHandler handler)
        {
            _subscribers.Add(handler);
        }
    }
}