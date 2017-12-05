namespace CqrsRadio.Domain.Events
{
    public struct RadioSongWithDeezerSongIdParsed : IDomainEvent
    {
        public readonly string RadioName;
        public readonly string SongId;

        public RadioSongWithDeezerSongIdParsed(string radioName, string songId)
        {
            RadioName = radioName;
            SongId = songId;
        }
    }
}