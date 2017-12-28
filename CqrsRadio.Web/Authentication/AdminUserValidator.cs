using System.Collections.Generic;
using System.Linq;
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

        public AdminUserValidator(IHmacProvider hmacProvider, IAdminRepository adminRepository)
        {
            _hmacProvider = hmacProvider;
            _adminRepository = adminRepository;
        }
        public IUserIdentity Validate(string username, string password)
        {
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
