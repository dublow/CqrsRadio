using CqrsRadio.ApiInfrastructure.ViewModel;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.ApiInfrastructure.Modules
{
    public class SongModule : NancyModule
    {
        public SongModule(ISongRepository songRepository) : base("Song")
        {
            Get["/GetRandomSongs/{size:int}"] = parameters =>
            {
                var size = (int) parameters.size;
                var songs = songRepository.GetRandomSongs(size);

                return Response.AsJson(songs);
            };

            Post["/Add"] = _ =>
            {
                var model = this.Bind<AddSong>();
                songRepository.Add(
                    UserId.Parse(model.UserId), 
                    PlaylistId.Parse(model.PlaylistId),
                    SongId.Parse(model.SongId), 
                    model.Title, model.Artist);

                return Response.AsJson(new { success = true }); ;
            };

            
        }
    }
}
