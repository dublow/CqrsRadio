using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct SongAdded : IDomainEvent
    {
        public readonly UserId UserId;
        public readonly string PlaylistName;
        public readonly string Title;
        public readonly string Artist;

        public SongAdded(UserId userId, string playlistName, string title, string artist)
        {
            UserId = userId;
            PlaylistName = playlistName;
            Title = title;
            Artist = artist;
        }
    }
}