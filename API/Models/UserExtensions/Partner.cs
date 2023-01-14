using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineFoodDelivery.Models.UserExtensions
{
    [Index(nameof(UserId), IsUnique = true)]
    public class Partner : IAccountDetails
    {
        public int Id { get; set; }

        public required string UserId { get; set; }
        public User? User { get; set; }
    }
}