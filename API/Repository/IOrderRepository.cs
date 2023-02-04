using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Repository
{
    public interface IOrderRepository : IGenericRepository<Order, Order, UpdateOrderDto>
    {
        Task<List<Order>> GetAllAsync(string userId);

        // Recommendation service helpers
        Task<List<Restaurant>> GetOrderedRestaurants(string userId);
        Task<List<EntityCounterDto<Restaurant>>> GetFrequentlyOrderedRestaurants(string userId);

        Task<List<Product>> GetOrderedProducts(string userId);
        Task<List<EntityCounterDto<Product>>> GetFrequentlyOrderedProducts(string userId);
    }
}