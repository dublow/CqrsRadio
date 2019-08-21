using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Test.Mocks;
using CqrsRadio.Web.Modules;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;


namespace CqrsRadio.Test.Modules
{
    [TestFixture]
    public class HomeModuleTest
    {
        [Test]
        public void WhenGetHomeReturnOk()
        {
            var browser = new Browser(config =>
            {
                config.Module<HomeModule>();
                config.Dependency(DeezerApiBuilder.Create().Build());
                config.Dependency(SongRepositoryBuilder.Create().Build());
                config.Dependency(PlaylistRepositoryBuilder.Create().Build());
                config.Dependency(new RadioEnvironment
                {
                    Name = EnvironmentType.Local,
                    AppId = "12345",
                    Channel = "channel"
                });
                config.Dependency(MetricBuilder.Create().Build());
            });

            var response = browser.Get("/");

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void WhenPostCreatePlaylistReturnOk()
        {
            var browser = new Browser(config =>
            {
                config.Module<HomeModule>();
                config.Dependency(DeezerApiBuilder.Create().SetCreatePlaylist(PlaylistId.Parse("123456789")).Build());
                config.Dependency(SongRepositoryBuilder.Create().Build());
                config.Dependency(PlaylistRepositoryBuilder.Create().SetCanCreatePlaylist(true).Build());
                config.Dependency(new RadioEnvironment
                {
                    Name = EnvironmentType.Local,
                    AppId = "12345",
                    Channel = "channel"
                });
                config.Dependency(MetricBuilder.Create().Build());
            });

            var response = browser.Post("/createPlaylist", context =>
            {
                context.FormValue("AccessToken", "12345");
                context.FormValue("UserId", "12");
                context.FormValue("Nickname", "nicolas");
                context.FormValue("Email", "nd@email.com");
                context.FormValue("PlaylistName", "pytName");
            });

            var body = JObject.Parse(response.Body.AsString());
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(true, body["playlistCreated"].Value<bool>());
        }

        [Test]
        public void WhenGetCanCreatePlaylistHomeReturnOk()
        {
            var browser = new Browser(config =>
            {
                config.Module<HomeModule>();
                config.Dependency(DeezerApiBuilder.Create().Build());
                config.Dependency(SongRepositoryBuilder.Create().Build());
                config.Dependency(PlaylistRepositoryBuilder.Create().SetCanCreatePlaylist(true).Build());
                config.Dependency(new RadioEnvironment
                {
                    Name = EnvironmentType.Local,
                    AppId = "12345",
                    Channel = "channel"
                });
                config.Dependency(MetricBuilder.Create().Build());
            });

            var response = browser.Get("/canCreatePlaylist/123");
            var body = JObject.Parse(response.Body.AsString());
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(true, body["canCreatePlaylist"].Value<bool>());
        }
    }
}
