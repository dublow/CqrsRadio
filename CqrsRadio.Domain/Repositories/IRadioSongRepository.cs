using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface IRadioSongRepository : IRepository
    {
        void Add(SongId songId, string radioName, string title, string artist);
        void AddToDuplicate(string radioName, string title, string artist);
        void AddToError(string radioName, string error);
        bool SongExists(SongId songId);
    }
}