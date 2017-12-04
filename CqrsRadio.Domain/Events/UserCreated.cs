using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct UserCreated : IDomainEvent
    {
        public readonly Identity Identity;

        public UserCreated(Identity identity)
        {
            Identity = identity;
        }
    }
}