using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public class RadioSongWithDeezerSongIdParsed : IDomainEvent, IEquatable<RadioSongWithDeezerSongIdParsed>
    {
        public readonly string RadioName;
        public readonly SongId SongId;
        public readonly string Title;
        public readonly string Artist;

        public RadioSongWithDeezerSongIdParsed(string radioName, SongId songId, string title, string artist)
        {
            RadioName = radioName;
            SongId = songId;
            Title = title;
            Artist = artist;
        }

        public static bool operator ==(RadioSongWithDeezerSongIdParsed left, RadioSongWithDeezerSongIdParsed right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RadioSongWithDeezerSongIdParsed left, RadioSongWithDeezerSongIdParsed right)
        {
            return !left.Equals(right);
        }

        public bool Equals(RadioSongWithDeezerSongIdParsed other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(RadioName, other.RadioName, StringComparison.InvariantCultureIgnoreCase) &&
                   Equals(SongId, other.SongId) &&
                   string.Equals(Title, other.Title, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Artist, other.Artist, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RadioSongWithDeezerSongIdParsed) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (RadioName != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(RadioName) : 0);
                hashCode = (hashCode * 397) ^ (SongId != null ? SongId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Title != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Title) : 0);
                hashCode = (hashCode * 397) ^ (Artist != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Artist) : 0);
                return hashCode;
            }
        }
    }
}