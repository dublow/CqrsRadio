using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Main.Handlers
{
    public class SongTimelineHandler<TEvent> : IHandler<SongAdded> where TEvent : IDomainEvent
    {
        public List<Song> Songs { get; }

        public SongTimelineHandler()
        {
            Songs = new List<Song>();
        }

        public void Handle(SongAdded evt)
        {
            Songs.Add(new Song(evt.Title, evt.Artist));
        }
    }
}