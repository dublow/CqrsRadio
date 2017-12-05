namespace CqrsRadio.Domain.Entities
{
    public struct DeezerSong
    {
        public readonly string Id;
        public readonly string Genre;
        public readonly string Title;
        public readonly string Artist;

        public DeezerSong(string id, string genre, string title, string artist)
        {
            Id = id;
            Genre = genre;
            Title = title;
            Artist = artist;
        }
    }
}