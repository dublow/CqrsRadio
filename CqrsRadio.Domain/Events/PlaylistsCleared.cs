using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Events
{
    public struct PlaylistsCleared : IDomainEvent
    {
        public readonly UserId UserId;

        public PlaylistsCleared(UserId userId)
        {
            UserId = userId;
        }
    }
}
