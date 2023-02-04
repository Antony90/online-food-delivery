using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Util;

namespace OnlineFoodDelivery.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<List<Restaurant>> GetOrderedRestaurants(string userId)
        {
            return await _context.Order
                .Where(o => o.UserId == userId)
                .OrderBy(o => o.OrderDate)
                .Select(o => o.Restaurant)
                .ToListAsync();
        }

        public async Task<List<EntityCounterDto<Restaurant>>> GetFrequentlyOrderedRestaurants(string userId)
        {
            return await _context.Order
                .Where(o => o.UserId == userId)
                .GroupBy(o => o.RestaurantId)
                .OrderByDescending(group => group.Count())
                .Select(group =>
                    new EntityCounterDto<Restaurant>
                    {
                        Obj = group.First().Restaurant,
                        Count = group.Count()
                    })
                .ToListAsync();
        }

        public async Task<List<Product>> GetOrderedProducts(string userId)
        {
            var productIds = _context.Order
                .Where(o => o.UserId == userId)
                .OrderBy(o => o.OrderDate)
                .SelectMany(o => o.Items)
                .Select(oi => oi.ProductId);


            var products = await productIds.Join(_context.Product,
                pId => pId, // outer select
                p => p.Id, // inner select
                (pId, p) => p // join row (we only care about the product model)
            ).ToListAsync();

            return products;
        }

        public async Task<List<EntityCounterDto<Product>>> GetFrequentlyOrderedProducts(string userId)
        {
            var productIdsByCountDesc = _context.Order
                .Where(o => o.UserId == userId)
                .OrderBy(o => o.OrderDate)
                .SelectMany(o => o.Items)
                .Select(oi => oi.ProductId)
                .GroupBy(x => x) // group by product ID e.g. group key 5 => [5, 5, 5, 5];
                .OrderByDescending(g => g.Count());

            var frequentProducts = await productIdsByCountDesc.Join(_context.Product,
                pIdGroup => pIdGroup.Key, // outer select, product ID
                p => p.Id, // inner select
                (pIdGroup, p) =>
                    new EntityCounterDto<Product>
                    {
                        Obj = p,
                        Count = pIdGroup.Count()
                    }
            ).ToListAsync();

            return frequentProducts;
        }


        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> DeleteAsync(int id)
        {
            var order = await _context.Order.FirstOrDefaultAsync(p => p.Id == id);

            if (order == null)
                return null;

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Order.Include(r => r.Items).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Order.FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Order.FindAsync(id) != null;
        }


        public async Task<Order?> UpdateAsync(int id, UpdateOrderDto updateDto)
        {
            var order = await _context.Order.FirstOrDefaultAsync(p => p.Id == id);

            if (order == null)
            {
                return null;
            }

            ObjectProps.Copy(updateDto, order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetAllAsync(string userId)
        {
            return await _context.Order.Where(o => o.UserId == userId)
                .ToListAsync();
        }

    }
}