using System.Runtime.Serialization.Formatters;
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
            container.Register<ISongRepository, SongRepository>();
        }
    }
}
