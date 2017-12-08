using System.Collections.Generic;
using Newtonsoft.Json;

namespace CqrsRadio.Deezer.Models
{
    public class TrackDeezer
    {
        public readonly int Total;
        public readonly string Next;
        [JsonProperty("data")]
        public readonly IEnumerable<TrackItemDeezer> Tracks;

        public TrackDeezer(int total, string next, IEnumerable<TrackItemDeezer> tracks)
        {
            Total = total;
            Next = next;
            Tracks = tracks;
        }
    }

    public class TrackItemDeezer
    {
        public readonly string Id;
        public readonly string Title;
        public readonly TrackItemArtistDeezer Artist;

        public TrackItemDeezer(string id, string title, TrackItemArtistDeezer artist)
        {
            Id = id;
            Title = title;
            Artist = artist;
        }
    }

    public class TrackItemArtistDeezer
    {
        public readonly string Name;

        public TrackItemArtistDeezer(string name)
        {
            Name = name;
        }
    }
}
