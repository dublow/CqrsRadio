using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public class AccessTokenAdded : IDomainEvent, IEquatable<AccessTokenAdded>
    {
        public readonly UserId UserId;

        public AccessTokenAdded(UserId userId)
        {
            UserId = userId;
        }

        public bool Equals(AccessTokenAdded other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UserId.Equals(other.UserId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AccessTokenAdded) obj);
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }

        public static bool operator ==(AccessTokenAdded left, AccessTokenAdded right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AccessTokenAdded left, AccessTokenAdded right)
        {
            return !Equals(left, right);
        }
    }
}
