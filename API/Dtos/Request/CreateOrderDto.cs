using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos.Request
{
    public class CreateOrderDto
    {
        public required string Address { get; set; }
        public double DestinationLong { get; set; }
        public double DestinationLat { get; set; }
        public required List<string> CouponCodes { get; set; }
        public string FoodPrepInstructions { get; set; } = string.Empty;
        public string DeliveryInstructions { get; set; } = string.Empty;
    }
}