using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct PlaylistDeleted : IDomainEvent, IEquatable<PlaylistDeleted>
    {
        public readonly UserId UserId;
        public readonly PlaylistId PlaylistId;
        public readonly string Name;
        public PlaylistDeleted(UserId userId, PlaylistId playlistId, string name)
        {
            UserId = userId;
            PlaylistId = playlistId;
            Name = name;
        }

        public bool Equals(PlaylistDeleted other)
        {
            return UserId.Equals(other.UserId) && PlaylistId.Equals(other.PlaylistId) && string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PlaylistDeleted && Equals((PlaylistDeleted) obj);
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

        public static bool operator ==(PlaylistDeleted left, PlaylistDeleted right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlaylistDeleted left, PlaylistDeleted right)
        {
            return !left.Equals(right);
        }
    }
}