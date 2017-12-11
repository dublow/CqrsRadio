using System;
using CqrsRadio.Domain.Utilities;

namespace CqrsRadio.Domain.ValueTypes
{
    public class Email : IEquatable<Email>
    {
        public readonly string Value;

        private Email(string value)
        {
            Value = value;
        }

        public static Email Parse(string value)
        {
            if(!StringUtilities.IsValidEmail(value))
                throw new ArgumentException("Invalid email");

            return new Email(value);
        }

        public static Email Empty
            => new Email(string.Empty);

        public bool IsEmpty
            => string.IsNullOrEmpty(Value);

        public bool Equals(Email other)
        {
            return string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Email && Equals((Email) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value) : 0);
        }

        public static bool operator ==(Email left, Email right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Email left, Email right)
        {
            return !left.Equals(right);
        }

    }
}
