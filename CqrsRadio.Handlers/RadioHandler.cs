using System;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Repositories;

namespace CqrsRadio.Handlers
{
    public class RadioHandler : IRadioHandler
    {
        private readonly IRadioRepository _radioRepository;

        public RadioHandler(IRadioRepository radioRepository)
        {
            _radioRepository = radioRepository;
        }

        public void Handle(RadioCreated evt)
        {
            _radioRepository.Create(evt.Name, evt.Url);
        }

        public void Handle(RadioDeleted evt)
        {
            _radioRepository.Delete(evt.Name);
        }
    }
}