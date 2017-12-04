using System;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Repositories;

namespace CqrsRadio.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserRepository _userRepository;

        public UserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Handle(UserCreated evt)
        {
            _userRepository.Create(evt.Identity.Email.Value, evt.Identity.Nickname.Value,
                evt.Identity.UserId.Value.ToString());
        }

        public void Handle(UserDeleted evt)
        {
            throw new NotImplementedException();
        }
    }
}