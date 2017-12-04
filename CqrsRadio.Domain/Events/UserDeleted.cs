using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct UserDeleted : IDomainEvent
    {
        public readonly UserId UserId;

        public UserDeleted(UserId userId)
        {
            UserId = userId;
        }
    }
}