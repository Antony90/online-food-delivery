using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Dtos
{
    public class CreateRestaurantDto
    {
        public required string Name { get; set; }
        public required int PartnerUserId { get; set; }
        public required double LocationLong { get; set; }
        public required double LocationLat { get; set; }

        public Restaurant ToRestaurant()
        {
            return new Restaurant
            {
                Name = this.Name,
                // PartnerId = this.PartnerUserId,
                LocationLong = this.LocationLong,
                LocationLat = this.LocationLat,
                Reviews = [],
                Menu = []
            };
        }
    }
}