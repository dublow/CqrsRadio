using System;
using System.Collections.Generic;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Entities
{
    public class Playlist : IEquatable<Playlist>
    {
        public readonly PlaylistId PlaylistId;
        public readonly string Name;
        public readonly List<Song> Songs;

        public Playlist(PlaylistId playlistId, string name)
        {
            PlaylistId = playlistId;
            Name = name;
            Songs = new List<Song>();
        }

        public static Playlist Empty
            => new Playlist(PlaylistId.Empty, string.Empty);

        public bool IsEmpty
            => string.IsNullOrEmpty(Name);

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public bool Equals(Playlist other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return PlaylistId.Equals(other.PlaylistId) && string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && Equals(Songs, other.Songs);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Playlist) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PlaylistId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^ (Songs != null ? Songs.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Playlist left, Playlist right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Playlist left, Playlist right)
        {
            return !Equals(left, right);
        }
    }
}