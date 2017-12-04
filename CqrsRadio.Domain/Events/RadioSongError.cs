namespace CqrsRadio.Domain.Events
{
    public struct RadioSongError : IDomainEvent
    {
        public readonly string RadioName;
        public readonly string Error;

        public RadioSongError(string radioName, string error)
        {
            RadioName = radioName;
            Error = error;
        }
    }
}
