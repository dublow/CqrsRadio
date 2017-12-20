using System.IO;
using System.Text;
using CqrsRadio.Common.Net;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Infrastructure.Providers;
using CqrsRadio.Infrastructure.Repositories;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using Nancy.TinyIoc;
using Newtonsoft.Json.Linq;
using NLog;

namespace CqrsRadio.Web
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Environment _environment;

        public NancyBootstrapper(Environment environment)
        {
            _environment = environment;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            container.Register(_environment);
            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();

            container.Register((cContainer, overloads) =>
                _environment.Name == EnvironmentType.Production 
                    ? (IProvider) new MonoSqliteProvider() 
                    : new SqliteProvider());

            container.Register<ISongRepository, SongRepository>();
            container.Register<IPlaylistRepository, PlaylistRepository>();

            pipelines.OnError += (ctx, ex) =>
            {
                Logger.Error(ex);
                var isAjaxRequest = ctx.Request.IsAjaxRequest();

                if (isAjaxRequest)
                {
                    var result = new JObject
                    {
                        {"status", "Fail"},
                        { "data",  "Internal error"}
                    };

                    var newContent = result.ToString();
                    var output = Encoding.UTF8.GetBytes(newContent);

                    return new Response
                    {
                        Contents = stream => stream.Write(output, 0, output.Length),
                        ContentType = "application/json",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                }

                return null;
            };

            pipelines.AfterRequest += (ctx) =>
            {
                var isAjaxRequest = ctx.Request.IsAjaxRequest();
                var isJsonResponse = ctx.Response.ContentType.Contains("json");

                if (isAjaxRequest && isJsonResponse)
                {
                    using (var memStream = new MemoryStream())
                    {
                        ctx.Response.Contents.Invoke(memStream);
                        var textResponse = Encoding.UTF8.GetString(memStream.ToArray());

                        var rr = JObject.Parse(textResponse);

                        var result = new JObject
                        {
                            {"status", ctx.Response.StatusCode.ToString()},
                            { "data",  rr}
                        };

                        var newContent = result.ToString();
                        var output = Encoding.UTF8.GetBytes(newContent);
                        
                        ctx.Response.Contents = stream => stream.Write(output, 0, output.Length);
                    }
                }
            };
        }
    }
}
