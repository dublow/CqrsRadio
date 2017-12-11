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
            _userRepository.Create(evt.Identity.Email, evt.Identity.Nickname,
                evt.Identity.UserId);
        }

        public void Handle(UserDeleted evt)
        {
            _userRepository.Delete(evt.UserId);
        }
    }
}