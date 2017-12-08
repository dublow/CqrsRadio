using System;

namespace CqrsRadio.Domain.ValueTypes
{
    public struct PlaylistId : IEquatable<PlaylistId>
    {
        public readonly string Value;

        public PlaylistId(string value)
        {
            Value = value;
        }

        public static PlaylistId Parse(string value)
        {
            if (!long.TryParse(value, out var valueAsInt))
                throw new ArgumentException("Invalid playlistId", value);

            return new PlaylistId(valueAsInt.ToString());
        }

        public static PlaylistId Empty => new PlaylistId();

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

        public static implicit operator PlaylistId(string value)
        {
            return Parse(value);
        }
    }
}