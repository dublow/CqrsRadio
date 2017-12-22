using CqrsRadio.Domain.Repositories;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.Cryptography;
using Nancy.ModelBinding;
using Nancy.Security;

namespace CqrsRadio.Web.Modules
{
    public class AdminModule : NancyModule
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IHmacProvider _hmacProvider;

        public AdminModule(IAdminRepository adminRepository, IHmacProvider hmacProvider) : base("admin")
        {
            _adminRepository = adminRepository;
            _hmacProvider = hmacProvider;
            //this.RequiresAuthentication();

            Get["/"] = _ => "admin";

            Get["/AddUser"] = _ => View["addUser"];

            Post["/addUser"] = parameter =>
            {
                var model = this.Bind<AddUserViewModel>();

                var hmac = _hmacProvider.GenerateHmac(model.Password);
                return Response.AsRedirect("/admin");
            };
        }
    }
}
