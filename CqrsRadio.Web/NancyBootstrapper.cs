using System;
using System.IO;
using System.Linq;
using System.Text;
using CqrsRadio.Common.AssemblyScanner;
using CqrsRadio.Common.Net;
using CqrsRadio.Common.StatsD;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Infrastructure.Providers;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Cryptography;
using Nancy.Extensions;
using Nancy.TinyIoc;
using Newtonsoft.Json.Linq;
using NLog;
using Environment = CqrsRadio.Domain.Configuration.Environment;

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
            Register(container);
            RegisterRepository(container);
            RegisterCrypo(container);

            pipelines.OnError += (ctx, ex) =>
            {
                container.Resolve<IMetric>().Count("error");
                Logger.Error(ex);
                var isAjaxRequest = ctx.Request.IsAjaxRequest();

                if (isAjaxRequest)
                {
                    var result = new JObject
                    {
                        {"isSuccess", false},
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
                            {"isSuccess", true},
                            { "data",  rr}
                        };

                        var newContent = result.ToString();
                        var output = Encoding.UTF8.GetBytes(newContent);
                        
                        ctx.Response.Contents = stream => stream.Write(output, 0, output.Length);
                    }
                }
            };
        }

        private void Register(TinyIoCContainer container)
        {
            container.Register(_environment);
            container.Register<IStatsDRequest>(new StatsDRequest("127.0.0.1", 8125));
            container.Register<IMetric, Metric>();
            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();
            container.Register((cContainer, overloads) => _environment.Name == EnvironmentType.Production
                ? (IProvider)new MonoSqliteProvider() : new SqliteProvider());
        }

        private void RegisterRepository(TinyIoCContainer container)
        {
            TypeScanner
                .GetTypesOf<IRepository>()
                .ForEach(type =>
                {
                    var interfaceType = type
                        .GetInterfaces()
                        .First(x => x != typeof(IRepository));

                    var instance = Activator
                        .CreateInstance(type, container.Resolve<IProvider>());

                    container.Register(interfaceType, instance);
                });
        }

        private void RegisterCrypo(TinyIoCContainer container)
        {
            var keyGenerator = new PassphraseKeyGenerator("forayer globular arse diminish highball wineskin",
                new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            var hmacProvider = new DefaultHmacProvider(keyGenerator);

            container.Register<IHmacProvider>(hmacProvider);
        }
    }
}
