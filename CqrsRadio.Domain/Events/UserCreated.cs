using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public class UserCreated : IDomainEvent, IEquatable<UserCreated>
    {
        public readonly Identity Identity;

        public UserCreated(Identity identity)
        {
            Identity = identity;
        }

        public bool Equals(UserCreated other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Identity.Equals(other.Identity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserCreated) obj);
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public static bool operator ==(UserCreated left, UserCreated right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserCreated left, UserCreated right)
        {
            return !Equals(left, right);
        }
    }
}