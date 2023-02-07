using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos.Request
{
    public class CreateDeliveryDto
    {
        public required string DeliveryInstructions { get; set; }
    }
}