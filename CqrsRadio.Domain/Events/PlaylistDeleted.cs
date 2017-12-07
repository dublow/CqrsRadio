using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct PlaylistDeleted : IDomainEvent
    {
        public readonly UserId UserId;
        public readonly PlaylistId PlaylistId;
        public readonly string Name;
        public PlaylistDeleted(UserId userId, PlaylistId playlistId, string name)
        {
            UserId = userId;
            PlaylistId = playlistId;
            Name = name;
        }
    }
}