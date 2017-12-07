using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Entities
{
    public struct Song
    {
        public readonly SongId SongId;
        public readonly string Title;
        public readonly string Artist;

        public Song(SongId songId, string title, string artist)
        {
            SongId = songId;
            Title = title;
            Artist = artist;
        }
    }
}
