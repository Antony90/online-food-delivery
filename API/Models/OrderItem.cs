using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineFoodDelivery.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderItem : Item
    {
        public int OrderId { get; set; }
    }
}