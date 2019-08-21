using CqrsRadio.ApiInfrastructure.ViewModel;
using CqrsRadio.Domain.Repositories;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.ApiInfrastructure.Modules
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IAdminRepository adminRepository) : base("Admin")
        {
            Post["/Add"] = _ =>
            {
                var model = this.Bind<AddAdmin>();

                if (!adminRepository.Exists(model.Login))
                {
                    adminRepository.Add(model.Login, model.Password);
                }

                return Response.AsJson(new{success = true, result = string.Empty});
            };

            Get["/GetPassword/{login}"] = parameters =>
            {
                var login = (string) parameters.login;

                return Response.AsJson(new { success = true, result = adminRepository.GetPassword(login) });
            };

            Get["/Exists/{login}"] = parameters =>
            {
                var login = (string) parameters.login;

                return Response.AsJson(new {success = true, result = adminRepository.Exists(login)});
            };
        }
    }
}
