using System;
using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
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
            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();
            if (Type.GetType("Mono.Runtime") != null)
                RegisterMono(container);
            else
                Register(container);


        }

        private void RegisterMono(TinyIoCContainer container)
        {
            container.Register<ISongRepository, MonoSongRepository>();
            container.Register<IPlaylistRepository, MonoPlaylistRepository>();
        }

        private void Register(TinyIoCContainer container)
        {
            container.Register<ISongRepository, SongRepository>();
            container.Register<IPlaylistRepository, PlaylistRepository>();
        }
    }
}
