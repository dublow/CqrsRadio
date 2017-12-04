namespace CqrsRadio.Domain.Events
{
    public struct SongAdded : IDomainEvent
    {
        public readonly string PlaylistName;
        public readonly string Title;
        public readonly string Artist;

        public SongAdded(string playlistName, string title, string artist)
        {
            PlaylistName = playlistName;
            Title = title;
            Artist = artist;
        }
    }
}