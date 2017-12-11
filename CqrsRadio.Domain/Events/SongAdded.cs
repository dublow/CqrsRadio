using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public class SongAdded : IDomainEvent, IEquatable<SongAdded>
    {
        public readonly UserId UserId;
        public readonly PlaylistId PlaylistId;
        public readonly SongId SongId;
        public readonly string Title;
        public readonly string Artist;

        public SongAdded(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist)
        {
            UserId = userId;
            PlaylistId = playlistId;
            SongId = songId;
            Title = title;
            Artist = artist;
        }

        public bool Equals(SongAdded other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UserId.Equals(other.UserId) && PlaylistId.Equals(other.PlaylistId) && SongId.Equals(other.SongId) &&
                   string.Equals(Title, other.Title, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Artist, other.Artist, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SongAdded) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UserId.GetHashCode();
                hashCode = (hashCode * 397) ^ PlaylistId.GetHashCode();
                hashCode = (hashCode * 397) ^ SongId.GetHashCode();
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Title);
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Artist);
                return hashCode;
            }
        }

        public static bool operator ==(SongAdded left, SongAdded right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SongAdded left, SongAdded right)
        {
            return !Equals(left, right);
        }
    }
}