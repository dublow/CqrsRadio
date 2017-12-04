using System;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Repositories;

namespace CqrsRadio.Handlers
{
    public class RadioSongHandler : IRadioSongHandler
    {
        private readonly IRadioSongRepository _radioSongRepository;

        public RadioSongHandler(IRadioSongRepository radioSongRepository)
        {
            _radioSongRepository = radioSongRepository;
        }

        public void Handle(RadioSongParsed evt)
        {
            _radioSongRepository.Add(evt.RadioName, evt.Title, evt.Artist);
        }

        public void Handle(RadioSongDuplicate evt)
        {
            throw new NotImplementedException();
        }

        public void Handle(RadioSongError evt)
        {
            throw new NotImplementedException();
        }
    }
}