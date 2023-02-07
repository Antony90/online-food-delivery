using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Enums;

namespace OnlineFoodDelivery.Dtos.Request
{
    public class UpdateDeliveryDto
    {
        public DeliveryStatus DeliveryStatus { get; set; }

        public required string DeliveryInstructions { get; set; }

        public double LiveLocationLat { get; set; }
        public double LiveLocationLong { get; set; }
    }
}