using System.Collections.Generic;
using Newtonsoft.Json;

namespace CqrsRadio.Deezer.Models
{
    public class PlaylistDeezer
    {
        public readonly int Total;
        public readonly string Next;
        [JsonProperty("data")]
        public readonly IEnumerable<PlaylistItemDeezer> Playlists;

        public PlaylistDeezer(int total, string next, IEnumerable<PlaylistItemDeezer> playlists)
        {
            Total = total;
            Next = next;
            Playlists = playlists;
        }
    }

    public class PlaylistItemDeezer
    {
        public readonly string Id;
        public readonly string Title;

        public PlaylistItemDeezer(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
