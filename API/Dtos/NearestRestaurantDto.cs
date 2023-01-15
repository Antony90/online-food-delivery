using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos
{
    public class NearestRestaurantDto
    {
        public double Distance { get; set; }

        public double LocationLat { get; set; }

        public double LocationLong { get; set; }

        public Vector2 ToLocationVec()
        {
            return new Vector2((float)this.LocationLong, (float)this.LocationLat);
        }
    }
}