namespace CqrsRadio.Domain.Events
{
    public struct RadioDeleted : IDomainEvent
    {
        public readonly string Name;

        public RadioDeleted(string name)
        {
            Name = name;
        }
    }
}