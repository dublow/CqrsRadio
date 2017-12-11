using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public class RadioSongWithDeezerSongIdParsed : IDomainEvent, IEquatable<RadioSongWithDeezerSongIdParsed>
    {
        public readonly string RadioName;
        public readonly SongId SongId;

        public RadioSongWithDeezerSongIdParsed(string radioName, SongId songId)
        {
            RadioName = radioName;
            SongId = songId;
        }

        public bool Equals(RadioSongWithDeezerSongIdParsed other)
        {
            return string.Equals(RadioName, other.RadioName, StringComparison.InvariantCultureIgnoreCase) && SongId.Equals(other.SongId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RadioSongWithDeezerSongIdParsed && Equals((RadioSongWithDeezerSongIdParsed) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.InvariantCultureIgnoreCase.GetHashCode(RadioName) * 397) ^ SongId.GetHashCode();
            }
        }

        public static bool operator ==(RadioSongWithDeezerSongIdParsed left, RadioSongWithDeezerSongIdParsed right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RadioSongWithDeezerSongIdParsed left, RadioSongWithDeezerSongIdParsed right)
        {
            return !left.Equals(right);
        }
    }
}