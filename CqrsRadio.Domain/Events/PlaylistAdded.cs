using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct PlaylistAdded : IDomainEvent
    {
        public readonly UserId UserId;
        public readonly string Name;

        public PlaylistAdded(UserId userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
