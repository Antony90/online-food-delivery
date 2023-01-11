using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Enums;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Repository
{
    public interface IRestaurantRepository : IGenericRepository<Restaurant, CreateRestaurantDto, UpdateRestaurantDto>
    {
        Task<List<Restaurant>> GetByCuisine(CuisineType cuisineTypeEnum);
        Task<decimal> GetRestaurantRevenue(int id);
    }
}