using System;

namespace CqrsRadio.Domain.Entities
{
    public struct DeezerSong : IEquatable<DeezerSong>
    {
        public readonly string Id;
        public readonly string Genre;
        public readonly string Title;
        public readonly string Artist;

        public DeezerSong(string id, string genre, string title, string artist)
        {
            Id = id;
            Genre = genre;
            Title = title;
            Artist = artist;
        }

        public static DeezerSong Empty =>
            new DeezerSong();

        public bool Equals(DeezerSong other)
        {
            return string.Equals(Id, other.Id) && string.Equals(Genre, other.Genre) && string.Equals(Title, other.Title) && string.Equals(Artist, other.Artist);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DeezerSong && Equals((DeezerSong) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Genre != null ? Genre.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Artist != null ? Artist.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(DeezerSong left, DeezerSong right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeezerSong left, DeezerSong right)
        {
            return !left.Equals(right);
        }
    }
}