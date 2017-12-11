using System;

namespace CqrsRadio.Domain.ValueTypes
{
    public class SongId : IEquatable<SongId>
    {
        public readonly string Value;

        private SongId(string value)
        {
            Value = value;
        }

        public static SongId Parse(string value)
        {
            if (!long.TryParse(value, out var valueAsInt))
                throw new ArgumentException("Invalid songId", value);

            return new SongId(valueAsInt.ToString());
        }

        public static SongId Empty 
            => new SongId(string.Empty);

        public bool IsEmpty 
            => string.IsNullOrEmpty(Value);

        public bool Equals(SongId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SongId && Equals((SongId)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(SongId left, SongId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SongId left, SongId right)
        {
            return !left.Equals(right);
        }
    }
}