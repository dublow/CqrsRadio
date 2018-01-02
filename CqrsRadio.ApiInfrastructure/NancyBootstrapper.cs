using System;
using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Infrastructure.Providers;
using CqrsRadio.Infrastructure.Providers.Dbs;
using CqrsRadio.Infrastructure.Repositories;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace CqrsRadio.ApiInfrastructure
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        private EnvironmentType Environment => Type.GetType("Mono.Runtime") != null
            ? EnvironmentType.Production
            : EnvironmentType.Local;

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            if (Environment == EnvironmentType.Local)
                RegisterPersistence(container);
            else
                RegisterMonoPersistence(container);

            CreateDomain(container);
            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();
        }

        private void RegisterMonoPersistence(TinyIoCContainer container)
        {
            var songProvider = new MonoProvider("monosong.sqlite");
            var domainProvider = new MonoProvider("monodomain.sqlite");
            var dbParameters = new MonoCustomDbParameter();

            container.Register<IProvider>(songProvider, "song");
            container.Register<IProvider>(domainProvider, "domain");
            container.Register<ISongRepository>(new SongRepository(songProvider, dbParameters));
            container.Register<IRadioSongRepository>(new RadioSongRepository(songProvider, dbParameters));
            container.Register<IAdminRepository>(new AdminRepository(domainProvider, dbParameters));
            container.Register<IPlaylistRepository>(new PlaylistRepository(domainProvider, dbParameters));
        }

        private void RegisterPersistence(TinyIoCContainer container)
        {
            var songProvider = new Provider("song.sqlite");
            var domainProvider = new Provider("domain.sqlite");
            var dbParameters = new CustomDbParameter();

            container.Register<IProvider>(songProvider, "song");
            container.Register<IProvider>(domainProvider, "domain");
            container.Register<ISongRepository>(new SongRepository(songProvider, dbParameters));
            container.Register<IRadioSongRepository>(new RadioSongRepository(songProvider, dbParameters));
            container.Register<IAdminRepository>(new AdminRepository(domainProvider, dbParameters));
            container.Register<IPlaylistRepository>(new PlaylistRepository(domainProvider, dbParameters));
        }

        private void CreateDomain(TinyIoCContainer container)
        {
            var domain = new Infrastructure.Providers.Dbs.Domain();
            domain.CreateDomain(container.Resolve<IProvider>("domain"));
            domain.CreateSong(container.Resolve<IProvider>("song"));
        }
    }
}
