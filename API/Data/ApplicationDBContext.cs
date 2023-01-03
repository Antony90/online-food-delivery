using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Models.UserExtensions;

namespace OnlineFoodDelivery.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
        : base(dbContextOptions)
        {

        }



        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Partner> PartnerUser { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<BasketItem> BasketItem { get; set; }

        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<DeliveryDriver> DeliveryDriver { get; set; }

        public DbSet<Coupon> Coupon { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<Order>()
            //     .HasMany(o => o.OrderItems)
            //     .WithOne(oi => oi.Order)
            //     .HasForeignKey(oi => oi.OrderId)
            //     .IsRequired();

            // Manually create 
            builder.Entity<DeliveryDriver>()
                .HasOne(dd => dd.User);

            // builder.Entity<Delivery>()
            //     .HasOne(d => d.Order)
            //     .WithOne(o => o.Delivery)
            //     .OnDelete(DeleteBehavior.NoAction);

            List<IdentityRole> roles =
            [
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Name = "Partner",
                    NormalizedName = "PARTNER"
                },
                new IdentityRole
                {
                    Name = "DeliveryDriver",
                    NormalizedName = "DELIVERYDRIVER"
                }
            ];

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}