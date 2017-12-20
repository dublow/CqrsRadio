using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Infrastructure.Providers;
using CqrsRadio.Infrastructure.Repositories;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace CqrsRadio.Web
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        private readonly Environment _environment;

        public NancyBootstrapper(Environment environment)
        {
            _environment = environment;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();

            container.Register((cContainer, overloads) =>
                _environment.Name == EnvironmentType.Production 
                    ? (IProvider) new MonoSqliteProvider() 
                    : new SqliteProvider());

            container.Register<ISongRepository, SongRepository>();
            container.Register<IPlaylistRepository, PlaylistRepository>();
        }
    }
}
