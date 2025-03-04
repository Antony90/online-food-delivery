using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



public class OrderCreatedEvent : IntegrationEvent
{
    public int OrderId { get; }
    public string CustomerName { get; }

    public OrderCreatedEvent(int orderId, string customerName)
    {
        OrderId = orderId;
        CustomerName = customerName;
    }
}