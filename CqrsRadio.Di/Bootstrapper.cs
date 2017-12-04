using CqrsRadio.Domain.EventStores;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using Unity;

namespace CqrsRadio.Di
{
    public class Bootstrapper
    {
        private readonly IUnityContainer _container;
        public Bootstrapper(IUnityContainer container)
        {
            var stream = new MemoryEventStream();
            var eventBus = new EventBus(stream);

            container.RegisterInstance<IEventStream>(stream);
            container.RegisterInstance(eventBus);

            _container = container;
        }
        
        public static Bootstrapper Initialize()
        {
            return new Bootstrapper(new UnityContainer());
        }

        public T Get<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
