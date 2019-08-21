using System;
using System.Linq;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using Newtonsoft.Json;

namespace CqrsRadio.Radio
{
    public class DjamRadioEngine : IRadioEngine
    {
        private readonly IRequest _request;

        public DjamRadioEngine(IRequest request)
        {
            _request = request;
        }

        public RadioSong Parse(Uri url)
        {
            return _request.Get(url.ToString(), s =>
            {
                var result = JsonConvert.DeserializeObject<DjamResult>(s);
                if (result != null && result.Tracks.Any() && result.Tracks.First().Deezer != null)
                {
                    var song = result.Tracks.First();
                    return RadioSong.Create(song.Title, song.Artist, SongId.Parse(song.Deezer.Id));

                }

                return RadioSong.Empty;
            });
        }

        public class DjamResult
        {
            public DjamTrack[] Tracks { get; set; }
        }
        public class DjamTrack
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public DjamDeezer Deezer { get; set; }
        }

        public class DjamDeezer
        {
            public string Id { get; set; }
        }
    }
}
