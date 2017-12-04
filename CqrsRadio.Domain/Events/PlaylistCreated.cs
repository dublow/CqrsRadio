namespace CqrsRadio.Domain.Events
{
    public struct PlaylistCreated : IDomainEvent
    {
        public readonly string UserId;
        public readonly string Name;

        public PlaylistCreated(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}