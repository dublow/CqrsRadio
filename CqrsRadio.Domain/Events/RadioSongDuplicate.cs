using System;

namespace CqrsRadio.Domain.Events
{
    public class RadioSongDuplicate : IDomainEvent, IEquatable<RadioSongDuplicate>
    {
        public readonly string RadioName;
        public readonly string Title;
        public readonly string Artist;

        public RadioSongDuplicate(string radioName, string title, string artist)
        {
            RadioName = radioName;
            Title = title;
            Artist = artist;
        }

        public bool Equals(RadioSongDuplicate other)
        {
            return string.Equals(RadioName, other.RadioName, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Title, other.Title, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Artist, other.Artist, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RadioSongDuplicate && Equals((RadioSongDuplicate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(RadioName);
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Title);
                hashCode = (hashCode * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Artist);
                return hashCode;
            }
        }

        public static bool operator ==(RadioSongDuplicate left, RadioSongDuplicate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RadioSongDuplicate left, RadioSongDuplicate right)
        {
            return !left.Equals(right);
        }
    }
}
