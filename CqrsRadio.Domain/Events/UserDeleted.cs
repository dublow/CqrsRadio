using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public class UserDeleted : IDomainEvent, IEquatable<UserDeleted>
    {
        public readonly UserId UserId;

        public UserDeleted(UserId userId)
        {
            UserId = userId;
        }

        public bool Equals(UserDeleted other)
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
            return Equals((UserDeleted) obj);
        }

        public override int GetHashCode()
        {
            return UserId.Value.GetHashCode();
        }

        public static bool operator ==(UserDeleted left, UserDeleted right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserDeleted left, UserDeleted right)
        {
            return !Equals(left, right);
        }
    }
}