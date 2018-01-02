using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using Nancy.Authentication.Basic;
using Nancy.Cryptography;
using Nancy.Security;

namespace CqrsRadio.Web.Authentication
{
    public class AdminUserValidator : IUserValidator
    {
        private readonly IHmacProvider _hmacProvider;
        private readonly IAdminRepository _adminRepository;
        private readonly Environment _environment;

        public AdminUserValidator(IHmacProvider hmacProvider, IAdminRepository adminRepository, Environment environment)
        {
            _hmacProvider = hmacProvider;
            _adminRepository = adminRepository;
            _environment = environment;
        }
        public IUserIdentity Validate(string username, string password)
        {
            if(_environment.Name == EnvironmentType.Local)
                return new AdminUserIdentity("localhost");

            var hash = _hmacProvider.GenerateHmac(password);
            var expectedHash = _adminRepository.GetPassword(username);

            if (!expectedHash.SequenceEqual(hash))
                return null;

            return new AdminUserIdentity(username);
        }
    }

    public class AdminUserIdentity : IUserIdentity
    {
        public string UserName { get; }
        public IEnumerable<string> Claims => new[] {"admin"};

        public AdminUserIdentity(string userName)
        {
            UserName = userName;
        }
    }
}
