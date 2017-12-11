using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Services;
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
        }
    }
}
