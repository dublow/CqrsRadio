using CqrsRadio.ApiInfrastructure.ViewModel;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.ApiInfrastructure.Modules
{
    public class RadioSongModule : NancyModule
    {
        public RadioSongModule(IRadioSongRepository radioSongRepository) : base("RadioSong")
        {
            Post["/Add"] = _ =>
            {
                var model = this.Bind<AddRadioSong>(new BindingConfig {BodyOnly = false});

                radioSongRepository.Add(SongId.Parse(model.SongId), model.RadioName, model.Title, model.Artist);

                return Response.AsJson(new { success = true });
            };

            Get["/SongExists/{songId}"] = parameters =>
            {
                var songId = SongId.Parse((string) parameters.songId);

                var songExists = radioSongRepository.SongExists(songId);
                return Response.AsJson(new { success = true, result = songExists });
            };
        }
    }
}
