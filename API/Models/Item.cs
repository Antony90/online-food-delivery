using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Models
{
    /* An instance of a product in a basket or order */
    public class Item
    {
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; } // The price when it was ordered/added to basket
        public int Quantity { get; set; }
    }
}