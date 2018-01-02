using CqrsRadio.Domain.Configuration;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.ViewEngines;

namespace CqrsRadio.Web.Handlers.StatusCode
{
    public class Status404CodeHandler : DefaultViewRenderer, IStatusCodeHandler
    {
        private readonly Environment _environment;
        public Status404CodeHandler(IViewFactory factory, Environment environment) : base(factory)
        {
            _environment = environment;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound
                   && _environment.Name == EnvironmentType.Production;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = RenderView(context, "Errors/404");
            response.StatusCode = HttpStatusCode.NotFound;
            context.Response = response;
        }
    }

    public class Status500CodeHandler : DefaultViewRenderer, IStatusCodeHandler
    {
        private readonly Environment _environment;
        public Status500CodeHandler(IViewFactory factory, Environment environment) : base(factory)
        {
            _environment = environment;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError 
                && context.Response.ContentType.Contains("html")
                && _environment.Name == EnvironmentType.Production;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = RenderView(context, "Errors/500");
            response.StatusCode = HttpStatusCode.InternalServerError;
            context.Response = response;
        }
    }
}
