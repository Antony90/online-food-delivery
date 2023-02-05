using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Enums;

namespace OnlineFoodDelivery.Models
{
    [Index(nameof(DeliveryDriverId), IsUnique = false)]
    public class Delivery
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int DeliveryDriverId { get; set; }
        public DeliveryDriver DeliveryDriver { get; set; }


        public DeliveryStatus DeliveryStatus { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }

        public required string DeliveryInstructions { get; set; }

        public double LiveLocationLat { get; set; }
        public double LiveLocationLong { get; set; }

        public DateTime? DeliveredAt { get; set; }
        public int? CustomerRating { get; set; }
    }
}