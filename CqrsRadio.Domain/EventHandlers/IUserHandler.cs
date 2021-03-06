﻿using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    public interface IUserHandler : 
        IHandler<UserCreated>, 
        IHandler<UserDeleted>
    { }
}
