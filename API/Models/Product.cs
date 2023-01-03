using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Enums;

namespace OnlineFoodDelivery.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public int RestaurantId { get; set; }
        public List<Order> Orders { get; set; } = [];

        public ProductCategory Category { get; set; }
    }
}