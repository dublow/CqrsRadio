namespace CqrsRadio.Domain.Repositories
{
    public interface IRadioSongRepository
    {
        void Add(string radioName, string title, string artist);
    }
}