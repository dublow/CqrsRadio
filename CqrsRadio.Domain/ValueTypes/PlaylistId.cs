using System;

namespace CqrsRadio.Domain.ValueTypes
{
    public class PlaylistId : IEquatable<PlaylistId>
    {
        public readonly string Value;

        private PlaylistId(string value)
        {
            Value = value;
        }

        public static PlaylistId Parse(string value)
        {
            if (!long.TryParse(value, out var valueAsInt))
                throw new ArgumentException("Invalid playlistId", value);

            return new PlaylistId(valueAsInt.ToString());
        }

        public static PlaylistId Empty 
            => new PlaylistId(string.Empty);

        public bool IsEmpty =>
            string.IsNullOrEmpty(Value);

        public bool Equals(PlaylistId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PlaylistId && Equals((PlaylistId)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(PlaylistId left, PlaylistId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlaylistId left, PlaylistId right)
        {
            return !left.Equals(right);
        }
    }
}