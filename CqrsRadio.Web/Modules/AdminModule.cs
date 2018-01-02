using System.IO;
using System.Linq;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
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
        public AdminModule(IHmacProvider hmacProvider, IRequest request, IAdminRepository adminRepository) : base("admin")
        {
            this.RequiresAuthentication();

            Get["/"] = _ => View["index"];

            Get["/AddUser"] = _ => View["addUser"];

            Post["/addUser"] = parameter =>
            {
                var model = this.Bind<AddUserViewModel>();
                var password = hmacProvider.GenerateHmac(model.Password);

                adminRepository.Add(model.Login, password);
                
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

            
        }
    }
}
