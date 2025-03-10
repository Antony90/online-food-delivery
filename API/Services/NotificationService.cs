using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



public class NotificationService : IEventHandler<OrderCreatedEvent>
{
    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        await Task.Delay(100);
        Console.WriteLine($"Notification sent for Order {@event.OrderId} to customer {@event.CustomerName}");
    }
}