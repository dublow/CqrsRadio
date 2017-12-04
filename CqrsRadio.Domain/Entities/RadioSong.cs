namespace CqrsRadio.Domain.Entities
{
    public struct RadioSong
    {
        public readonly string Title;
        public readonly string Artist;

        private RadioSong(string title, string artist)
        {
            Title = title;
            Artist = artist;
        }

        public static RadioSong Create(string title, string artist)
        {
            return new RadioSong(title, artist);
        }
    }
}
