using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Entities
{
    public struct Song
    {
        public readonly SongId SongId;
        public readonly string Genre;
        public readonly string Title;
        public readonly string Artist;

        public Song(SongId songId, string genre, string title, string artist)
        {
            SongId = songId;
            Genre = genre;
            Title = title;
            Artist = artist;
        }
    }
}
