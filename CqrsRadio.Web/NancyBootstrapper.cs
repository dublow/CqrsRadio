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
using CqrsRadio.Infrastructure.Persistences;
using CqrsRadio.Infrastructure.Providers;
using CqrsRadio.Infrastructure.Providers.Dbs;
using CqrsRadio.Web.Authentication;
using Nancy;
using Nancy.Authentication.Basic;
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
            if (_environment.Name == EnvironmentType.Production)
                RegisterMonoPersistence(container);
            else
                RegisterPersistence(container);


            
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

            var hmacProvider = container.Resolve<IHmacProvider>();
            var adminRepository = container.Resolve<IAdminRepository>();
            var adminUserValidator = new AdminUserValidator(hmacProvider, adminRepository, _environment);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(adminUserValidator, "admin"));
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
            var v = TypeScanner
                .GetTypesOf<IRepository>();

                v.ForEach(type =>
                {
                    var interfaceType = type
                        .GetInterfaces()
                        .First(x => x != typeof(IRepository));

                    var isRadioSong = typeof(ISongRepository) == interfaceType 
                                      || typeof(IRadioSongRepository) == interfaceType;

                    var instance = Activator
                        .CreateInstance(type, container.Resolve<IProvider>(isRadioSong ? "song" : "domain"),
                            container.Resolve<IDbParameter>());

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

        private void RegisterMonoPersistence(TinyIoCContainer container)
        {
            container.Register<IProvider, MonoDomainProvider>("domain");
            container.Register<IProvider, MonoSongProvider>("song");
            container.Register<IDbParameter, MonoCustomDbParameter>();
            container.Register(new DatabaseDomain(new MonoDomainProvider()));
            container.Register(new DatabaseSong(new MonoSongProvider(),
                container.Resolve<IDeezerApi>()));
        }

        private void RegisterPersistence(TinyIoCContainer container)
        {
            container.Register<IProvider, DomainProvider>("domain");
            container.Register<IProvider, SongProvider>("song");
            container.Register<IDbParameter, CustomDbParameter>();
            container.Register(new DatabaseDomain(new DomainProvider()));
            container.Register(new DatabaseSong(new SongProvider(),
                container.Resolve<IDeezerApi>()));
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
