using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IEventBus
{
    void Publish<T>(T @event) where T : IntegrationEvent;
    void Subscribe<T, TH>() where T : IntegrationEvent where TH : IEventHandler<T>;
}