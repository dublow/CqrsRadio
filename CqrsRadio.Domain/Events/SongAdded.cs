using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct SongAdded : IDomainEvent
    {
        public readonly UserId UserId;
        public readonly string PlaylistName;
        public readonly SongId SongId;
        public readonly string Genre;
        public readonly string Title;
        public readonly string Artist;

        public SongAdded(UserId userId, string playlistName, SongId songId, string genre, string title, string artist)
        {
            UserId = userId;
            PlaylistName = playlistName;
            SongId = songId;
            Genre = genre;
            Title = title;
            Artist = artist;
        }
    }
}