using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;

namespace CqrsRadio.Handlers
{
    public class RadioSongHandler : IRadioSongHandler
    {
        private readonly IRadioSongRepository _radioSongRepository;
        private readonly IDeezerApi _deezerApi;

        public RadioSongHandler(IRadioSongRepository radioSongRepository, IDeezerApi deezerApi)
        {
            _radioSongRepository = radioSongRepository;
            _deezerApi = deezerApi;
        }

        public void Handle(RadioSongParsed evt)
        {
            var song = _deezerApi.GetSong(evt.Title, evt.Artist);
            StoreSong(evt.RadioName, song);
        }

        public void Handle(RadioSongWithDeezerSongIdParsed evt)
        {
            var song = _deezerApi.GetSong(evt.SongId);
            StoreSong(evt.RadioName, song);
        }

        public void Handle(RadioSongDuplicate evt)
        {
            _radioSongRepository.AddToDuplicate(evt.RadioName, evt.Title, evt.Artist);
        }

        public void Handle(RadioSongError evt)
        {
            _radioSongRepository.AddToError(evt.RadioName, evt.Error);
        }

        private void StoreSong(string radioName, DeezerSong song)
        {
            if (song != DeezerSong.Empty)
                _radioSongRepository.Add(song.Id, song.Genre, radioName, song.Title, song.Artist);
        }
    }
}