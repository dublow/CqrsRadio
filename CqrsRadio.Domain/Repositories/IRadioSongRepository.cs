namespace CqrsRadio.Domain.Repositories
{
    public interface IRadioSongRepository
    {
        void Add(string songId, string genre, string radioName, string title, string artist);
        void AddToDuplicate(string radioName, string title, string artist);
        void AddToError(string radioName, string error);
    }
}