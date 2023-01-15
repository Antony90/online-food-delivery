using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos.Request
{
    public class UpdateRestaurantDto
    {
        public required string PartnerUserId { get; set; }
        public required string Name { get; set; }
        public double LocationLong { get; set; }
        public double LocationLat { get; set; }
    }
}