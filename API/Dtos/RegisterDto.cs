using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Dtos
{
    public class RegisterDto
    {
        public required string UserName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required double LocationLong { get; set; }
        public required double LocationLat { get; set; }
    }
}