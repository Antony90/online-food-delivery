using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineFoodDelivery.Models
{
    [PrimaryKey(nameof(UserId), nameof(ProductId))] // Composite key
    public class BasketItem : Item
    {
        // The user whose basket the item is in
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
    }

}