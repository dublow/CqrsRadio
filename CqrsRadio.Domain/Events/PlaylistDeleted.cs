namespace CqrsRadio.Domain.Events
{
    public struct PlaylistDeleted : IDomainEvent
    {
        public readonly string Name;
        public PlaylistDeleted(string name)
        {
            Name = name;
        }
    }
}