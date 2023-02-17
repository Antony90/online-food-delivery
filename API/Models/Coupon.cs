using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineFoodDelivery.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Coupon
    {
        public int Id { get; set; }
        public List<Order> Orders { get; set; } = [];

        public required string Code { get; set; }

        public float DiscountPercent { get; set; } = 0.0f;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal FlatDiscount { get; set; } = 0.0m;
        public bool FreeDelivery { get; set; } = false;

        public DateTime ValidUntil { get; set; }
    }
}