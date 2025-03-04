using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class InMemoryEventBus : IEventBus
{
    private readonly Dictionary<Type, List<Type>> _handlers = new Dictionary<Type, List<Type>>();
    private readonly IServiceProvider _serviceProvider;

    public InMemoryEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Publish<T>(T @event) where T : IntegrationEvent
    {
        var eventType = @event.GetType();
        if (_handlers.ContainsKey(eventType))
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var handlerType in _handlers[eventType])
                {
                    var handler = scope.ServiceProvider.GetService(handlerType) as IEventHandler<T>;
                    handler?.HandleAsync(@event);
                }
            }
        }
    }

    public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IEventHandler<T>
    {
        var eventType = typeof(T);
        var handlerType = typeof(TH);

        if (!_handlers.ContainsKey(eventType))
        {
            _handlers[eventType] = new List<Type>();
        }

        if (!_handlers[eventType].Contains(handlerType))
        {
            _handlers[eventType].Add(handlerType);
        }
    }
}