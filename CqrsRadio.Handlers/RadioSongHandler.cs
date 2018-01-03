using System;
using CqrsRadio.Common.StatsD;
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
        private readonly IMetric _metric;

        public RadioSongHandler(IRadioSongRepository radioSongRepository, IDeezerApi deezerApi, IMetric metric)
        {
            _radioSongRepository = radioSongRepository;
            _deezerApi = deezerApi;
            _metric = metric;
        }

        public void Handle(RadioSongParsed evt)
        {
            _metric.Count("radiosongparsed");
            //var song = _deezerApi.GetSong("accessToken", evt.Title, evt.Artist);
            //StoreSong(evt.RadioName, song);
        }

        public void Handle(RadioSongWithDeezerSongIdParsed evt)
        {
            //var song = _deezerApi.GetSong("", evt.SongId);

            if (!_radioSongRepository.SongExists(evt.SongId))
            {
                _metric.Count("radiosongwithdeezersongidparsed");
                StoreSong(evt.RadioName, new DeezerSong(evt.SongId, evt.Title, evt.Artist));
            }
            else
                _metric.Count("radiosongduplicate");
        }

        public void Handle(RadioSongDuplicate evt)
        {
            
            //_radioSongRepository.AddToDuplicate(evt.RadioName, evt.Title, evt.Artist);
        }

        public void Handle(RadioSongError evt)
        {
            _metric.Count("radiosongerror");
            //_radioSongRepository.AddToError(evt.RadioName, evt.Error);
        }

        private void StoreSong(string radioName, DeezerSong song)
        {
            if (song != DeezerSong.Empty)
                _radioSongRepository.Add(song.Id, radioName, song.Title, song.Artist);
        }
    }
}