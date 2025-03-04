using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IEventHandler<TEvent> where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent @event);
}

