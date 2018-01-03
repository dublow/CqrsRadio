using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Entities
{
    public class RadioSong : IEquatable<RadioSong>
    {
        public readonly string Title;
        public readonly string Artist;
        public readonly SongId SongId;

        private RadioSong(string title, string artist)
        {
            Title = title;
            Artist = artist;
            SongId = SongId.Empty;
        }

        private RadioSong(string title, string artist, SongId songId)
        {
            Title = title;
            Artist = artist;
            SongId = songId;
        }

        public static RadioSong Create(string title, string artist)
        {
            return new RadioSong(title, artist);
        }

        public static RadioSong Create(string title, string artist, SongId songId)
        {
            return new RadioSong(title, artist, songId);
        }

        public static RadioSong Empty => 
            new RadioSong(string.Empty, string.Empty);

        public bool Equals(RadioSong other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Title, other.Title, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Artist, other.Artist, StringComparison.InvariantCultureIgnoreCase) &&
                   Equals(SongId, other.SongId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RadioSong) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Title != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Title) : 0);
                hashCode = (hashCode * 397) ^ (Artist != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Artist) : 0);
                hashCode = (hashCode * 397) ^ (SongId != null ? SongId.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(RadioSong left, RadioSong right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RadioSong left, RadioSong right)
        {
            return !Equals(left, right);
        }
    }
}
