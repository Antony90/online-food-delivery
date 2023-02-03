using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Enums;

namespace OnlineFoodDelivery.Models
{
    [Index(nameof(OrderDate), IsUnique = false)]
    [Index(nameof(RestaurantId), IsUnique = false)]
    public class Order
    {
        public int Id { get; set; }
        public required string UserId { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; } = [];
        public Delivery Delivery { get; set; }
        public ICollection<Coupon> Coupons { get; set; } = [];

        public bool Cancelled { get; set; } = false;
        public OrderState OrderState { get; set; } = OrderState.Preparing;
        public DateTime OrderDate { get; set; } = default!;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountedPrice { get; set; }

        public required string Address { get; set; }
        public double DestinationLong { get; set; }
        public double DestinationLat { get; set; }

        public string FoodPrepInstructions { get; set; } = string.Empty;
    }
}