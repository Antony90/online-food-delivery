using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Util;

namespace OnlineFoodDelivery.Repository
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepository _orderRepo;

        public DeliveryRepository(ApplicationDBContext context, UserManager<User> userManager, IOrderRepository orderRepo)
        {
            _context = context;
            _userManager = userManager;
            _orderRepo = orderRepo;
        }


        public async Task SetUserRating(int deliveryId, int userRating)
        {
            if (userRating > 5 || userRating < 0)
            {
                throw new Exception("Rating must be in [0, 5].");
            }

            var delivery = await _context.Delivery
                .FirstOrDefaultAsync(d => d.Id == deliveryId)
                ?? throw new Exception("Invalid delivery ID: " + deliveryId);

            delivery.CustomerRating = userRating;
            await _context.SaveChangesAsync();

            // Update average rating for the related delivery driver
            var deliveryDriver = await _context.DeliveryDriver
                .FirstOrDefaultAsync(dd => dd.Id == delivery.DeliveryDriverId)
                ?? throw new Exception("Invalid user ID for delivery ID" + deliveryId);

            // Calculate new average rating and update driver
            deliveryDriver.AverageRating = await GetAverageUserRatingAsync(deliveryDriver.Id);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDeliveryRecipient(int deliveryId, string userId)
        {
            var delivery = await GetByIdAsync(deliveryId)
                ?? throw new Exception("Invalid delivery ID");

            var order = await _orderRepo.GetByIdAsync(delivery.OrderId);

            return order?.UserId == userId;
        }


        private async Task<double> GetAverageUserRatingAsync(int deliveryDriverId)
        {
            return await _context.Delivery.Where(d => d.DeliveryDriverId == deliveryDriverId)
                .AverageAsync(d => d.CustomerRating) ?? 0.0d; // 0 if no deliveries done
        }

        public async Task<Delivery> CreateAsync(string userId, int orderId, CreateDeliveryDto newObjDto)
        {
            // Get ordering user for destination location
            var order = await _context.Order.FindAsync(orderId)
                ?? throw new Exception("Invalid order ID");

            var restaurant = await _context.Restaurant.FindAsync(order.RestaurantId)
                ?? throw new Exception("Invalid restaurant ID");

            // Find related delivery driver
            var deliveryDriver = await _context.DeliveryDriver
                .FirstOrDefaultAsync(dd => dd.UserId == userId)
                ?? throw new Exception("Given user ID is not a delivery driver.");

            var delivery = new Delivery
            {
                DeliveryDriverId = deliveryDriver.Id,
                OrderId = orderId,
                DeliveryInstructions = newObjDto.DeliveryInstructions,
                EstimatedDeliveryTime = EstimateDeliveryArrivalTime(
                    restaurant.LocationLat, restaurant.LocationLong,
                    order.DestinationLong, order.DestinationLat)
            };

            await _context.Delivery.AddAsync(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<string?> GetDeliveryDriverUserId(int deliveryId)
        {
            var delivery = await _context.Delivery
                .Include(d => d.DeliveryDriver)
                .FirstOrDefaultAsync(d => d.Id == deliveryId);

            return delivery?.DeliveryDriver.UserId;
        }

        private static DateTime EstimateDeliveryArrivalTime(
            double currLocLong, double currLocLat, double destLong, double destLat)
        {
            return DateTime.Now;
        }

        public async Task<Delivery?> DeleteAsync(int id)
        {
            var delivery = await _context.Delivery.FirstOrDefaultAsync(d => d.Id == id);

            if (delivery == null)
                return null;

            _context.Delivery.Remove(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<List<Delivery>> GetAllAsync()
        {
            return await _context.Delivery.ToListAsync();
        }

        public async Task<Delivery?> GetByIdAsync(int id)
        {
            return await _context.Delivery.FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Delivery.FindAsync(id) != null;
        }


        public async Task<Delivery?> UpdateAsync(int id, UpdateDeliveryDto updateDto)
        {
            var delivery = await _context.Delivery.FirstOrDefaultAsync(p => p.Id == id);

            if (delivery == null)
            {
                return null;
            }

            var order = await _context.Order.FindAsync(delivery.OrderId);

            ObjectProps.Copy(updateDto, delivery);
            delivery.EstimatedDeliveryTime = EstimateDeliveryArrivalTime(
                updateDto.LiveLocationLong, updateDto.LiveLocationLat,
                order.DestinationLong, order.DestinationLat
            );

            await _context.SaveChangesAsync();

            return delivery;
        }


        public Task<Delivery> CreateAsync(CreateDeliveryDto newObjDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Delivery>> GetAllAsync(string userId)
        {
            return await _context.Delivery.Include(d => d.DeliveryDriver).Where(d => d.DeliveryDriver.UserId == userId)
                .ToListAsync();
        }


    }
}