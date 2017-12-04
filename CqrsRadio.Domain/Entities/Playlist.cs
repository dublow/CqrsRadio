using System;
using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Entities
{
    public struct Playlist : IEquatable<Playlist>
    {
        public readonly UserId UserId;
        public readonly string Name;
        public readonly List<Song> Songs;

        public Playlist(UserId userId, string name)
        {
            UserId = userId;
            Name = name;
            Songs = new List<Song>();
        }

        public void AddSong(Song song)
        {
            Songs.Add(song);
        }


        public bool Equals(Playlist other)
        {
            return string.Equals(UserId, other.UserId) && string.Equals(Name, other.Name) && Songs.SequenceEqual(other.Songs);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Playlist && Equals((Playlist) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Songs != null ? Songs.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}