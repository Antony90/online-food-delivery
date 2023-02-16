using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineFoodDelivery.Models
{
    /* Only store non-relational data
       A 1-to-(0 or 1) relationship, where the primary key is also the foreign key!
    */
    [Index(nameof(UserId), IsUnique = true)]
    public class DeliveryDriver : IAccountDetails
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public required string UserId { get; set; }
        public User? User { get; set; }

        public double LocationPrefLong { get; set; }
        public double LocationPrefLat { get; set; }
        public double AverageRating { get; set; }
    }
}