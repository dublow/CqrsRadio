using System;
using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
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
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            var isMono = Type.GetType("Mono.Runtime") != null;

            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();

            container.Register((cContainer, overloads) =>
                isMono ? (IProvider) new MonoSqliteProvider() : new SqliteProvider());

            container.Register<ISongRepository, SongRepository>();
            container.Register<IPlaylistRepository, PlaylistRepository>();
        }
    }
}
