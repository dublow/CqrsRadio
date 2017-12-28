using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.Cryptography;
using Nancy.ModelBinding;
using Nancy.Security;
using Newtonsoft.Json.Linq;

namespace CqrsRadio.Web.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IAdminRepository adminRepository, IHmacProvider hmacProvider) : base("admin")
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
                var jArray = new JArray();
                File
                    .ReadAllLines("/var/log/auth.log").Select(x =>
                    {
                        var dateIp = Parsor.Line(x);
                        return dateIp;
                    })
                    .Where(x => !x.IsEmpty)
                    .GroupBy(x => new {x.Date, x.Ip})
                    .Take(100)
                    .ToList()
                    .ForEach(x =>
                    {
                        var wr = WebRequest.Create($"http://ip-api.com/json/{x.Key.Ip}?fields=lat,lon");
                        using (var response = wr.GetResponse())
                        {
                            using (var sr = new StreamReader(response.GetResponseStream()))
                            {
                                var end = sr.ReadToEnd();
                                Console.WriteLine(end);
                                var parsed = JObject.Parse(end);

                                if (parsed.HasValues)
                                    jArray.Add(parsed);
                            }
                        }
                    });

                return Response.AsJson(jArray);
            };
        }
    }

    public static class Parsor
    {
        private static readonly Regex RxDate = new Regex(@"^((?:J[au]n|Feb|Ma[ry]|Apr|Jul|Aug|Sep|Oct|Nov|Dec) \s+ \d+)", RegexOptions.IgnorePatternWhitespace);
        private static readonly Regex RxIp = new Regex(@"((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))[.]((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))[.]((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))[.]((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))", RegexOptions.IgnorePatternWhitespace);

        public static DateIp Line(string line)
        {
            var matchDate = RxDate.Match(line);
            var matchIp = RxIp.Match(line);

            if(matchIp.Success && matchDate.Success)
                return DateIp.Create(matchDate.Value, matchIp.Value);

            return DateIp.Empty;
        }
    }

    public class DateIp
    {
        public readonly string Date;
        public readonly string Ip;
        public readonly bool IsEmpty;

        private DateIp(string date, string ip)
        {
            Date = date;
            Ip = ip;

            IsEmpty = string.IsNullOrEmpty(Date) 
                && string.IsNullOrEmpty(Ip);
        }

        public static DateIp Create(string date, string ip) => new DateIp(date, ip);
        public static DateIp Empty => new DateIp(string.Empty, String.Empty);
    }
}
