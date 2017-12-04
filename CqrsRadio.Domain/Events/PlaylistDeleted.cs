using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct PlaylistDeleted : IDomainEvent
    {
        public readonly UserId UserId;
        public readonly string Name;
        public PlaylistDeleted(UserId userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}