using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Enums;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public ProductCategory Category { get; set; }


        public Product ToProduct()
        {
            return new Product
            {
                Name = this.Name,
                Price = this.Price,
                RestaurantId = this.RestaurantId,
                Category = this.Category
            };
        }
    }
}
