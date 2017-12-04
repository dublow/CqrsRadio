using System.Collections.Generic;
using CqrsRadio.Domain.Events;

namespace CqrsRadio.Domain.EventStores
{
    public interface IEventStream
    {
        void Add(IDomainEvent evt);
        IEnumerable<IDomainEvent> GetEvents();
    }
}
