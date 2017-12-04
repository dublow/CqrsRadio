namespace CqrsRadio.Domain.Events
{
    public struct RadioSongDuplicate : IDomainEvent
    {
        public readonly string RadioName;
        public readonly string Title;
        public readonly string Artist;

        public RadioSongDuplicate(string radioName, string title, string artist)
        {
            RadioName = radioName;
            Title = title;
            Artist = artist;
        }
    }
}
