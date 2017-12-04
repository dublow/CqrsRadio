using System.Collections.Generic;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;

namespace CqrsRadio.Infrastructure.EventStores
{
    public class MemoryEventStream : IEventStream
    {
        private readonly IList<IDomainEvent> _events;

        public MemoryEventStream()
        {
            _events = new List<IDomainEvent>();
        }

        public void Add(IDomainEvent evt)
        {
            _events.Add(evt);
        }

        public IEnumerable<IDomainEvent> GetEvents()
        {
            return _events;
        }
    }
}
