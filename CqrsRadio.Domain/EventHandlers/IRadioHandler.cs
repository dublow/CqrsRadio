using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;

namespace CqrsRadio.Domain.EventHandlers
{
    public interface IRadioHandler :
        IHandler<RadioCreated>,
        IHandler<RadioDeleted>
    { }
}
