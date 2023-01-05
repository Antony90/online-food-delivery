using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos
{
    public class UpdateProductDto
    {
        public required string Name { get; set; }

        public decimal Price { get; set; }
    }
}