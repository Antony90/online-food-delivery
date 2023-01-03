using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Enums;
using OnlineFoodDelivery.Models.UserExtensions;

namespace OnlineFoodDelivery.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        // public int PartnerId { get; set; }
        // public Partner? Partner { get; set; }

        public required string Name { get; set; }

        public List<Product> Menu { get; set; } = [];

        public List<Review> Reviews { get; set; } = [];

        public List<Order> Orders { get; set; } = [];

        public CuisineType CuisineType { get; set; }

        public required double LocationLong { get; set; }
        public required double LocationLat { get; set; }

        public RestaurantDto ToDto()
        {
            return new RestaurantDto
            {
                Id = this.Id,
                Name = this.Name,
                LocationLat = this.LocationLat,
                LocationLong = this.LocationLong,
            };
        }

        public Vector2 ToLocationVec()
        {
            return new Vector2((float)this.LocationLong, (float)this.LocationLat);
        }
    }
}