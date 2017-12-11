using System;

namespace CqrsRadio.Domain.ValueTypes
{
    public class Nickname : IEquatable<Nickname>
    {
        public readonly string Value;
        private Nickname(string value)
        {
            Value = value;
        }

        public static Nickname Parse(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("Nickname is empty", value);

            return new Nickname(value.ToLower());
        }

        public static Nickname Empty
            => new Nickname(String.Empty);

        public bool IsEmpty
            => string.IsNullOrEmpty(Value);

        public bool Equals(Nickname other)
        {
            return string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Nickname && Equals((Nickname)obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value) : 0);
        }

        public static bool operator ==(Nickname left, Nickname right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Nickname left, Nickname right)
        {
            return !left.Equals(right);
        }
    }
}
