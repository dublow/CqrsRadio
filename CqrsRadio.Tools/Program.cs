using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Persistences;
using CqrsRadio.Infrastructure.Providers;
using CqrsRadio.Infrastructure.Repositories;

namespace CqrsRadio.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlLiteDb.CreateDomain();

            var provider = new SqliteProvider();

            var radioSongRepository = new RadioSongRepository(provider);
            var request = new RadioRequest();
            var deezerApi = new DeezerApi(request);

            var playlistIds = deezerApi.GetPlaylistIdsByUserId("frBQLuh4wfjFXawHtrKImjBzyPN5Pcvo0Gr5HoVSAzkVCiMy0kS", UserId.Parse("4934039"));

            foreach (var playlistId in playlistIds)
            {
                var songs = deezerApi.GetSongsByPlaylistId("frBQLuh4wfjFXawHtrKImjBzyPN5Pcvo0Gr5HoVSAzkVCiMy0kS", playlistId);

                foreach (var deezerSong in songs)
                {
                    if (!radioSongRepository.SongExists(deezerSong.Id))
                    {
                        radioSongRepository.Add(deezerSong.Id, "NUSED", deezerSong.Title, deezerSong.Artist);
                    }
                }
            }
        }
    }
}
