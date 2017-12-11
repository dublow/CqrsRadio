using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public class PlaylistAdded : IDomainEvent, IEquatable<PlaylistAdded>
    {
        public readonly UserId UserId;
        public readonly PlaylistId PlaylistId;
        public readonly string Name;

        public PlaylistAdded(UserId userId, PlaylistId playlistId, string name)
        {
            UserId = userId;
            PlaylistId = playlistId;
            Name = name;
        }

        public bool Equals(PlaylistAdded other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UserId.Equals(other.UserId) && PlaylistId.Equals(other.PlaylistId) && string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlaylistAdded) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UserId.GetHashCode();
                hashCode = (hashCode * 397) ^ PlaylistId.GetHashCode();
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name);
                return hashCode;
            }
        }

        public static bool operator ==(PlaylistAdded left, PlaylistAdded right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PlaylistAdded left, PlaylistAdded right)
        {
            return !Equals(left, right);
        }
    }
}
