using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Repository;

namespace OnlineFoodDelivery.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDBContext _context;
        private readonly IOrderRepository _orderRepo;
        private readonly IDeliveryRepository _deliveryRepo;

        public OrderService(ApplicationDBContext context, IOrderRepository orderRepo, IDeliveryRepository deliveryRepo)
        {
            _context = context;
            _orderRepo = orderRepo;
            _deliveryRepo = deliveryRepo;
        }


        public async Task<Order> CreateOrderFromItems(string userId, CreateOrderDto orderDto, IEnumerable<Item> items)
        {
            // Take items from user's item list, apply discount
            VerifyBasketResult verifyResult = await VerifyItems(items);
            List<Coupon?> coupons = await FindCouponCodes(orderDto.CouponCodes);
            List<Coupon> validCoupons = coupons.Where(c => c != null).ToList();

            var order = new Order
            {
                UserId = userId,
                RestaurantId = verifyResult.RestaurantId,
                TotalPrice = verifyResult.TotalPrice,
                DiscountedPrice = CalculateDiscountedPrice(verifyResult.TotalPrice, coupons),
                Coupons = validCoupons,
                Address = orderDto.Address,
                DestinationLong = orderDto.DestinationLong,
                DestinationLat = orderDto.DestinationLat,
                FoodPrepInstructions = orderDto.FoodPrepInstructions
            };

            var createdOrder = await _orderRepo.CreateAsync(order);

            // Create order items related to the order, from the basket
            var orderItems = ConvertToOrderItems(createdOrder.Id, items);
            await _context.OrderItem.AddRangeAsync(orderItems);

            await _deliveryRepo.CreateAsync(
                userId,
                createdOrder.Id,
                new CreateDeliveryDto { DeliveryInstructions = orderDto.DeliveryInstructions }
            );

            return order;

        }


        /* Create a new order using the same items as an existing order */
        public async Task<Order> ReOrderFromPrevious(int id, CreateOrderDto orderDto)
        {
            var prevOrder = await _orderRepo.GetByIdAsync(id)
                ?? throw new Exception("Order ID does not exist.");

            var newOrder = await CreateOrderFromItems(prevOrder.UserId, orderDto, prevOrder.Items);

            return newOrder;

        }



        private async Task<VerifyBasketResult> VerifyItems(IEnumerable<Item> items)
        {
            // Verify all items are from same restaurant
            // Accumulate total price too
            decimal totalPrice = 0.0m;
            var currentRestId = 0;
            foreach (var item in items)
            {
                var product = await _context.Product.FindAsync(item.ProductId)
                    ?? throw new Exception("Invalid item ID selected for order.");

                // Check if same as last and update loop var
                if (product.RestaurantId != currentRestId && currentRestId > 0)
                {
                    throw new Exception("Not all items from same restaurant.");
                }

                // Update if same or 0
                currentRestId = product.RestaurantId;
                totalPrice += product.Price;
            }

            return new VerifyBasketResult
            {
                RestaurantId = currentRestId,
                TotalPrice = totalPrice
            };
        }

        /* Create order items for the associated order ID, using basket items */
        private IEnumerable<OrderItem> ConvertToOrderItems(int orderId, IEnumerable<Item> items)
        {
            var orderItems = items.Select(bItem => new OrderItem
            {
                OrderId = orderId,
                ProductId = bItem.ProductId,
                Price = bItem.Price,
                Quantity = bItem.Quantity,
            });

            return orderItems;
        }


        private decimal CalculateDiscountedPrice(decimal preDiscountPrice, List<Coupon?> coupons)
        {
            foreach (var coupon in coupons)
            {
                if (coupon == null)
                {
                    continue;
                }
            }
            return 0.0m;
        }

        private async Task<List<Coupon?>> FindCouponCodes(List<string> couponCodes)
        {
            var coupons = couponCodes.Select(code =>
                _context.Coupon.FirstOrDefault(c => c.Code == code)).ToList();

            return coupons;
        }

        public Task<EmailResult> EmailReciept(int id)
        {
            throw new NotImplementedException();
        }


        private class VerifyBasketResult
        {
            public decimal TotalPrice;
            public int RestaurantId;
        }
    }
}