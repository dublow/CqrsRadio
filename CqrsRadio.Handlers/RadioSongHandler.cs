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

            if(song != DeezerSong.Empty)
                _radioSongRepository.Add(song.Id, song.Genre, evt.RadioName, song.Title, song.Artist);
        }

        public void Handle(RadioSongWithDeezerSongIdParsed radioSongParsed)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(RadioSongDuplicate evt)
        {
            _radioSongRepository.AddToDuplicate(evt.RadioName, evt.Title, evt.Artist);
        }

        public void Handle(RadioSongError evt)
        {
            _radioSongRepository.AddToError(evt.RadioName, evt.Error);
        }

        
    }
}