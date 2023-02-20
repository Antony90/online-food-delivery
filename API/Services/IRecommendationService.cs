using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Services
{
    public interface IRecommendationService
    {
        Task<List<Restaurant>> GetSimilarRestaurants(string userId);
        Task<RecommendationResult<Restaurant>> RecommendRestaurants(string userId);

        Task<List<Product>> GetSimilarProducts(string userId);
        Task<RecommendationResult<Product>> RecommendProducts(string userId);

        Task<RecommendationDto> GetUserRecommendations(string userId);
    }
}