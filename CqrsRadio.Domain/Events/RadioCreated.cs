using System;

namespace CqrsRadio.Domain.Events
{
    public struct RadioCreated : IDomainEvent
    {
        public readonly string Name;
        public readonly Uri Url;

        public RadioCreated(string name, Uri url)
        {
            Name = name;
            Url = url;
        }
    }
}