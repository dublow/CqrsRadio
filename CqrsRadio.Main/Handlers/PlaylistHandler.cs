using System;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Main.Handlers
{
    public class PlaylistHandler<TEvent> : IHandler<PlaylistCreated> where TEvent : IDomainEvent
    {
        public string Name { get; set; }
        public void Handle(PlaylistCreated evt)
        {
            Console.WriteLine($"Playlist created: {evt.Name}");
            Name = evt.Name;
        }
    }
}
