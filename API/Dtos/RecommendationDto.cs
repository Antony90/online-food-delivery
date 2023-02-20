using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Dtos
{
    public class RecommendationDto
    {
        public required RecommendationResult<Restaurant> Restaurant { get; set; }
        public required RecommendationResult<Product> Product { get; set; }
    }
}