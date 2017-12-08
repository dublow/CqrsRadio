using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct PlaylistAdded : IDomainEvent
    {
        public readonly UserId UserId;
        public readonly PlaylistId PlaylistId;
        public readonly string Name;

        public PlaylistAdded(UserId userId, PlaylistId playlistId, string name)
        {
            UserId = userId;
            PlaylistId = playlistId;
            Name = name;
        }
    }
}
