using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos.Request
{
    public class UpdateBasketDto
    {
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public bool? AddItem { get; set; }
    }
}