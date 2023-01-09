using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos
{
    /* Removes relational data */
    public class RestaurantDto
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public required double LocationLong { get; set; }

        public required double LocationLat { get; set; }

    }

}