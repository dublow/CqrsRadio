using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.Services;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace CqrsRadio.Web
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            container.Register<IEventStream, MemoryEventStream>();
            container.Register<IEventPublisher, EventBus>();
            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();
        }
    }
}
