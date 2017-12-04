using System;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Main.Handlers
{
    public class SongCounterHandler<TEvent> : IHandler<SongAdded> where TEvent : IDomainEvent
    {
        public int Value { get; private set; }
        public void Handle(SongAdded evt)
        {
            Console.WriteLine($"Music Added: {evt.Artist} {evt.Title}");
            Value++;
        }
    }
}