using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnlineFoodDelivery.Models
{
    public class User : IdentityUser, IAccountDetails
    {

        public required double LocationLong { get; set; }
        public required double LocationLat { get; set; }

        public ICollection<BasketItem> Basket { get; set; } = new List<BasketItem>();

        // Optional Role related properties

        // Delivery Driver
        // public Delivery

        public Vector2 ToLocationVec()
        {
            return new Vector2((float)this.LocationLong, (float)this.LocationLat);
        }
    }
}