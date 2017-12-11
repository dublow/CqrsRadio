using System;

namespace CqrsRadio.Domain.Events
{
    public class RadioSongError : IDomainEvent, IEquatable<RadioSongError>
    {
        public readonly string RadioName;
        public readonly string Error;

        public RadioSongError(string radioName, string error)
        {
            RadioName = radioName;
            Error = error;
        }

        public bool Equals(RadioSongError other)
        {
            return string.Equals(RadioName, other.RadioName, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Error, other.Error, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RadioSongError && Equals((RadioSongError) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.InvariantCultureIgnoreCase.GetHashCode(RadioName) * 397) ^
                       StringComparer.InvariantCultureIgnoreCase.GetHashCode(Error);
            }
        }

        public static bool operator ==(RadioSongError left, RadioSongError right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RadioSongError left, RadioSongError right)
        {
            return !left.Equals(right);
        }
    }
}
