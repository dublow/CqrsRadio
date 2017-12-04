using System;

namespace CqrsRadio.Domain.ValueTypes
{
    public struct Identity : IEquatable<Identity>
    {
        public readonly Email Email;
        public readonly Nickname Nickname;
        public readonly UserId UserId;

        private Identity(Email email, Nickname nickname, UserId userId)
        {
            Email = email;
            Nickname = nickname;
            UserId = userId;
        }

        public static Identity Create(string email, string nickname, string userId)
        {
            return new Identity(email, nickname, userId);
        }

        public bool Equals(Identity other)
        {
            return Email.Equals(other.Email) && Nickname.Equals(other.Nickname) && UserId.Equals(other.UserId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Identity && Equals((Identity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Email.GetHashCode();
                hashCode = (hashCode * 397) ^ Nickname.GetHashCode();
                hashCode = (hashCode * 397) ^ UserId.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Identity left, Identity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identity left, Identity right)
        {
            return !left.Equals(right);
        }
    }
}
