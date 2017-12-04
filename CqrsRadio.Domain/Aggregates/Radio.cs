using System;
using System.Linq;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.Services;

namespace CqrsRadio.Domain.Aggregates
{
    public class Radio  
    {
        private readonly IEventPublisher _publisher;
        private readonly Decision _decision;
        private readonly IRadioEngine _radioEngine;

        public string Name { get; private set; }
        public Uri Url { get; private set; }

        public Radio(IEventStream stream, IEventPublisher publisher, IRadioEngine radioEngine)
        {
            _publisher = publisher;
            _decision = new Decision(stream);
            _radioEngine = radioEngine;

            Restore(stream);
        }

        public static Radio Create(IEventStream stream, IEventPublisher publisher, IRadioEngine radioEngine, string name, string url)
        {
            var radio = new Radio(stream, publisher, radioEngine)
            {
                Name = name,
                Url = new Uri(url)
            };

            publisher.Publish(new RadioCreated(radio.Name, radio.Url));

            return radio;
        }

        public void Delete()
        {
            if (_decision.IsDeleted) return;

            PublishAndApply(new RadioDeleted(Name));
        }

        public void SearchSong()
        {
            if (_decision.IsDeleted) return;

            try
            {
                var song = _radioEngine.Parse(Url);

                if (_decision.RadioSongExists(Name, song))
                    _publisher.Publish(new RadioSongDuplicate(Name, song.Title, song.Artist));
                else
                    _publisher.Publish(new RadioSongParsed(Name, song.Title, song.Artist));
            }
            catch (Exception e)
            {
                _publisher.Publish(new RadioSongError(Name, e.Message));
            }
        }

        private void PublishAndApply(IDomainEvent evt)
        {
            _publisher.Publish(evt);
            _decision.Apply(evt);
        }

        private void Restore(IEventStream stream)
        {
            foreach (var domainEvent in stream.GetEvents())
            {
                if (domainEvent is RadioCreated radioCreated)
                {
                    Name = radioCreated.Name;
                    Url = radioCreated.Url;
                }
            }
        }

        private class Decision
        {
            private readonly IEventStream _stream;
            public bool IsDeleted { get; private set; }

            public Decision(IEventStream stream)
            {
                _stream = stream;
                foreach (var domainEvent in _stream.GetEvents())
                {
                    if (domainEvent is RadioDeleted radioDeleted)
                        Apply(radioDeleted);
                }
            }

            public void Apply(IDomainEvent evt)
            {
                IsDeleted = true;
            }

            public bool RadioSongExists(string name, RadioSong radioSong)
            {
                return _stream.GetEvents().OfType<RadioSongParsed>()
                    .Any(x => x == new RadioSongParsed(name, radioSong.Title, radioSong.Artist));
            }
        }
    }
}