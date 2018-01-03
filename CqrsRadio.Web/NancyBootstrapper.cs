using System;
using System.IO;
using System.Text;
using CqrsRadio.Common.Net;
using CqrsRadio.Common.StatsD;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Infrastructure.Repositories;
using CqrsRadio.Web.Authentication;
using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Bootstrapper;
using Nancy.Cryptography;
using Nancy.Extensions;
using Nancy.TinyIoc;
using Newtonsoft.Json.Linq;
using NLog;

namespace CqrsRadio.Web
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RadioEnvironment _environment;

        public NancyBootstrapper(RadioEnvironment environment)
        {
            _environment = environment;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            Register(container);
            RegisterRepository(container);
            RegisterCrypo(container);
            EnableBasicAuthentication(container, pipelines);
            EnableOnError(container, pipelines);
            EnableAfterRequest(pipelines);
        }

        private void Register(TinyIoCContainer container)
        {
            container.Register(_environment);
            container.Register<IStatsDRequest>(new StatsDRequest("127.0.0.1", 8125));
            container.Register<IMetric, Metric>();
            container.Register<IRequest, RadioRequest>();
            container.Register<IDeezerApi, DeezerApi>();
        }

        private void RegisterRepository(TinyIoCContainer container)
        {
            container.Register<ISongRepository, HttpSongRepository>();
            container.Register<IRadioSongRepository, HttpRadioSongRepository>();
            container.Register<IAdminRepository, HttpAdminRepository>();
            container.Register<IPlaylistRepository, HttpPlaylistRepository>();
        }

        private void RegisterCrypo(TinyIoCContainer container)
        {
            var keyGenerator = new PassphraseKeyGenerator("forayer globular arse diminish highball wineskin",
                new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            var hmacProvider = new DefaultHmacProvider(keyGenerator);

            container.Register<IHmacProvider>(hmacProvider);
        }

        private void EnableBasicAuthentication(TinyIoCContainer container, IPipelines pipelines)
        {
            var hmacProvider = container.Resolve<IHmacProvider>();
            var adminRepository = container.Resolve<IAdminRepository>();
            var adminUserValidator = new AdminUserValidator(hmacProvider, adminRepository, _environment);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(adminUserValidator, "admin"));
        }

        private void EnableOnError(TinyIoCContainer container, IPipelines pipelines)
        {
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
        }

        private void EnableAfterRequest(IPipelines pipelines)
        {
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

                        var result = new JObject();
                        if (TryParseJObject(textResponse, out var jo))
                        {
                            result = new JObject
                            {
                                {"isSuccess", true},
                                { "data", jo}
                            };
                        }
                        else if (TryParseJArray(textResponse, out var ja))
                        {
                            result = new JObject
                            {
                                {"isSuccess", true},
                                { "data", ja}
                            };
                        }
                        var newContent = result.ToString();
                        var output = Encoding.UTF8.GetBytes(newContent);

                        ctx.Response.Contents = stream => stream.Write(output, 0, output.Length);
                    }
                }
            };
        }

        private bool TryParseJObject(string value, out JObject jobject)
        {
            jobject = null;
            try
            {
                jobject = JObject.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TryParseJArray(string value, out JArray jarray)
        {
            jarray = null;
            try
            {
                jarray = JArray.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
