using System;

namespace CqrsRadio.Domain.ValueTypes
{
    public class Identity : IEquatable<Identity>
    {
        public readonly Email Email;
        public readonly Nickname Nickname;
        public readonly UserId UserId;
        public readonly string AccessToken;

        private Identity(Email email, Nickname nickname, UserId userId, string accessToken)
        {
            Email =  email;
            Nickname = nickname;
            UserId = userId;
            AccessToken = accessToken;
        }

        public static Identity Parse(string email, string nickname, string userId, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentException("Access token is empty", accessToken);

            return new Identity(Email.Parse(email), Nickname.Parse(nickname), UserId.Parse(userId), accessToken);
        }

        public static Identity Empty
            => new Identity(Email.Empty, Nickname.Empty, UserId.Empty, string.Empty);

        public bool IsEmpty
            => Email.IsEmpty || Nickname.IsEmpty || UserId.IsEmpty || string.IsNullOrEmpty(AccessToken);


        public bool Equals(Identity other)
        {
            return Email.Value.Equals(other.Email.Value) && Nickname.Value.Equals(other.Nickname.Value) && UserId.Value.Equals(other.UserId.Value) &&
                   string.Equals(AccessToken, other.AccessToken, StringComparison.InvariantCultureIgnoreCase);
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
                var hashCode = Email.Value.GetHashCode();
                hashCode = (hashCode * 397) ^ Nickname.Value.GetHashCode();
                hashCode = (hashCode * 397) ^ UserId.Value.GetHashCode();
                hashCode = (hashCode * 397) ^ (AccessToken != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(AccessToken) : 0);
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
