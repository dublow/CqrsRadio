using System;

namespace CqrsRadio.Domain.Events
{
    public class RadioCreated : IDomainEvent, IEquatable<RadioCreated>
    {
        public readonly string Name;
        public readonly Uri Url;

        public RadioCreated(string name, Uri url)
        {
            Name = name;
            Url = url;
        }

        public bool Equals(RadioCreated other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && Url.Equals(other.Url);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RadioCreated) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name) * 397) ^ Url.GetHashCode();
            }
        }

        public static bool operator ==(RadioCreated left, RadioCreated right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RadioCreated left, RadioCreated right)
        {
            return !Equals(left, right);
        }
    }
}