using Nancy;
using Nancy.ErrorHandling;
using Nancy.ViewEngines;

namespace CqrsRadio.Web.Handlers.StatusCode
{
    public class Status404CodeHandler : DefaultViewRenderer, IStatusCodeHandler
    {
        public Status404CodeHandler(IViewFactory factory) : base(factory)
        {
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
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
        public Status500CodeHandler(IViewFactory factory) : base(factory)
        {
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError 
                && context.Response.ContentType.Contains("html");
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = RenderView(context, "Errors/500");
            response.StatusCode = HttpStatusCode.InternalServerError;
            context.Response = response;
        }
    }
}
