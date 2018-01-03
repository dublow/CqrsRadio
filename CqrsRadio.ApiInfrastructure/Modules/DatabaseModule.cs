using System;
using CqrsRadio.ApiInfrastructure.ViewModel;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.ApiInfrastructure.Modules
{
    public class DatabaseModule : NancyModule
    {
        public DatabaseModule(IDeezerApi deezerApi, IRadioSongRepository radioSongRepository) : base("Database")
        {
            Post["/RestoreSong"] = _ =>
            {
                var model = this.Bind<RestoreSong>();

                var playlistIds = deezerApi
                    .GetPlaylistIdsByUserId(
                        model.AccessToken,
                        UserId.Parse(model.UserId),
                        s => s.ToLower().Contains("djam"));

                foreach (var playlistId in playlistIds)
                {
                    var songs = deezerApi.GetSongsByPlaylistId(model.AccessToken, playlistId);

                    foreach (var deezerSong in songs)
                    {
                        if (!radioSongRepository.SongExists(deezerSong.Id))
                        {
                            radioSongRepository.Add(deezerSong.Id, "NUSED", deezerSong.Title, deezerSong.Artist);
                        }
                    }
                }

                return Response.AsJson(new { success = true, result = string.Empty });
            };
        }
    }
}
