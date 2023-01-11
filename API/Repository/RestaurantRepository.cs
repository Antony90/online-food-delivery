using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Enums;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Util;

namespace OnlineFoodDelivery.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDBContext _context;

        public RestaurantRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Restaurant>> GetByCuisine(CuisineType cuisineType)
        {
            return await _context.Restaurant
                .Include(r => r.Orders)
                .Where(r => r.CuisineType == cuisineType)
                .OrderByDescending(r => r.Orders.ToList().Count())
                .ToListAsync();
        }

        public async Task<decimal> GetRestaurantRevenue(int id)
        {
            var restaurantOrders = _context.Order
                .Where(o => o.RestaurantId == id);

            return await restaurantOrders.SumAsync(o => o.DiscountedPrice);
        }


        public async Task<Restaurant> CreateAsync(CreateRestaurantDto newObjDto)
        {
            var restaurant = newObjDto.ToRestaurant();
            await _context.Restaurant.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }


        public async Task<Restaurant?> DeleteAsync(int id)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(p => p.Id == id);

            if (restaurant == null)
                return null;

            _context.Restaurant.Remove(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<List<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurant.Include(r => r.Menu).ToListAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            return await _context.Restaurant
                // .Include(r => r.Partner)
                .Include(r => r.Menu)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Restaurant.FindAsync(id) != null;
        }


        public async Task<Restaurant?> UpdateAsync(int id, UpdateRestaurantDto updateDto)
        {
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(p => p.Id == id);

            if (restaurant == null)
            {
                return null;
            }

            ObjectProps.Copy(updateDto, restaurant);

            await _context.SaveChangesAsync();

            return restaurant;
        }


    }
}