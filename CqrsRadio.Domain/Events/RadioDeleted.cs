using System;

namespace CqrsRadio.Domain.Events
{
    public class RadioDeleted : IDomainEvent, IEquatable<RadioDeleted>
    {
        public readonly string Name;

        public RadioDeleted(string name)
        {
            Name = name;
        }

        public bool Equals(RadioDeleted other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RadioDeleted && Equals((RadioDeleted) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name);
        }

        public static bool operator ==(RadioDeleted left, RadioDeleted right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RadioDeleted left, RadioDeleted right)
        {
            return !left.Equals(right);
        }
    }
}