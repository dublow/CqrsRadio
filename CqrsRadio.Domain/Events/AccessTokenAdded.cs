using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct AccessTokenAdded : IDomainEvent
    {
        public readonly UserId UserId;

        public AccessTokenAdded(UserId userId)
        {
            UserId = userId;
        }
    }
}
