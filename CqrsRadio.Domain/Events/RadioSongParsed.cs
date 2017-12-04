﻿using System;

namespace CqrsRadio.Domain.Events
{
    public struct RadioSongParsed : IDomainEvent, IEquatable<RadioSongParsed>
    {
        public readonly string Title;
        public readonly string Artist;

        public RadioSongParsed(string title, string artist)
        {
            Title = title;
            Artist = artist;
        }

        public bool Equals(RadioSongParsed other)
        {
            return string.Equals(Title, other.Title, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Artist, other.Artist, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RadioSongParsed && Equals((RadioSongParsed) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.InvariantCultureIgnoreCase.GetHashCode(Title) * 397) ^
                       StringComparer.InvariantCultureIgnoreCase.GetHashCode(Artist);
            }
        }

        public static bool operator ==(RadioSongParsed left, RadioSongParsed right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RadioSongParsed left, RadioSongParsed right)
        {
            return !left.Equals(right);
        }
    }
}