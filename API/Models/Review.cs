using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int? AuthorId { get; set; }
    }
}