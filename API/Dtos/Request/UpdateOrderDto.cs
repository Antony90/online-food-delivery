using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Enums;

namespace OnlineFoodDelivery.Dtos.Request
{
    public class UpdateOrderDto
    {
        public bool? Cancelled { get; set; }
        public decimal? TotalPrice { get; set; }
        public OrderState? OrderState { get; set; }
        public string? Address { get; set; }
        public double? DestinationLong { get; set; }
        public double? DestinationLat { get; set; }
    }
}