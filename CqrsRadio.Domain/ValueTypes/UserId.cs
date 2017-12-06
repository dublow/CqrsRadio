using System;

namespace CqrsRadio.Domain.ValueTypes
{
    public struct UserId : IEquatable<UserId>
    {
        public readonly string Value;

        public UserId(string value)
        {
            Value = value;
        }

        public static UserId Parse(string value)
        {
            if(!int.TryParse(value, out var valueAsInt))
                throw new ArgumentException("Invalid userId", value);

            return new UserId(valueAsInt.ToString());
        }

        public bool Equals(UserId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserId && Equals((UserId) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(UserId left, UserId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UserId left, UserId right)
        {
            return !left.Equals(right);
        }

        public static implicit operator UserId(string value)
        {
            return Parse(value);
        }
    }
}
