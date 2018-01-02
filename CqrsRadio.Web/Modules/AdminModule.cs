using System;
using System.IO;
using System.Linq;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Persistences;
using CqrsRadio.Web.Hack;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.Cryptography;
using Nancy.ModelBinding;
using Nancy.Security;

namespace CqrsRadio.Web.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IAdminRepository adminRepository, 
            IHmacProvider hmacProvider, IRequest request,
            DatabaseSong dbsong, DatabaseDomain dbDomain,
            IDeezerApi deezerApi, IRadioSongRepository radioSongRepository) : base("admin")
        {
            this.RequiresAuthentication();

            Get["/"] = _ => View["index"];

            Get["/AddUser"] = _ => View["addUser"];

            Post["/addUser"] = parameter =>
            {
                var model = this.Bind<AddUserViewModel>();

                if (!adminRepository.Exists(model.Login))
                {
                    adminRepository
                        .Add(
                            model.Login,
                            hmacProvider.GenerateHmac(model.Password));
                }

                return Response.AsRedirect("/admin");
            };

            Get["/GetCoordinate"] = _ =>
            {
                var hackManager = new HackManager(request);

                var flattenAsDateIp = File.ReadAllLines("/var/log/auth.log").Select(x =>
                    {
                        var dateIp = DateIpParsor.Line(x);
                        return dateIp;
                    })
                    .Where(x => !x.IsEmpty)
                    .GroupBy(x => new { x.Date, x.Ip })
                    .Select(x=>DateIp.Create(x.Key.Date, x.Key.Ip))
                    .ToList();

                var localizations = hackManager.GetLocalization(flattenAsDateIp);

                return Response.AsJson(localizations);
            };

            Get["GenerateSong"] = _ =>
            {
                dbsong.Create();
                var playlistIds = deezerApi
                    .GetPlaylistIdsByUserId(
                        "frKtbRGI9G18kljXooH4oQ0XbmntBD7oXeKBVBcVKIyjMMSDle0", 
                        UserId.Parse("4934039"), s => s.ToLower().Contains("djam"));

                
                foreach (var playlistId in playlistIds)
                {
                    var songs = deezerApi.GetSongsByPlaylistId("frKtbRGI9G18kljXooH4oQ0XbmntBD7oXeKBVBcVKIyjMMSDle0", playlistId);

                    foreach (var deezerSong in songs)
                    {
                        if (!radioSongRepository.SongExists(deezerSong.Id))
                        {
                            Console.WriteLine(deezerSong.Id);
                            radioSongRepository.Add(deezerSong.Id, "NUSED", deezerSong.Title, deezerSong.Artist);
                        }
                    }
                }

                return "ok";
            };

            Get["GenerateDomain"] = _ =>
            {
                dbDomain.Create();

                dbsong.Create();
                return "ok";
            };
        }
    }
}
