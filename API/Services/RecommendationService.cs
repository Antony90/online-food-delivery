using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Enums;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Repository;

namespace OnlineFoodDelivery.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ApplicationDBContext _context;
        private readonly IOrderRepository _orderRepo;

        public RecommendationService(ApplicationDBContext context, IOrderRepository orderRepo)
        {
            _context = context;
            _orderRepo = orderRepo;
        }

        public async Task<RecommendationDto> GetUserRecommendations(string userId)
        {
            return new RecommendationDto
            {
                Restaurant = await RecommendRestaurants(userId),
                Product = await RecommendProducts(userId)
            };
        }

        public async Task<RecommendationResult<Restaurant>> RecommendRestaurants(string userId)
        {
            return new RecommendationResult<Restaurant>
            {
                Similar = await GetSimilarRestaurants(userId),
                Frequented = await _orderRepo.GetFrequentlyOrderedRestaurants(userId)
            };
        }


        public async Task<List<Restaurant>> GetSimilarRestaurants(string userId)
        {
            var frequentlyOrderedRestaurantsDto = await _orderRepo.GetFrequentlyOrderedRestaurants(userId);
            var frequentlyOrderedRestaurants = frequentlyOrderedRestaurantsDto.Select(dto => dto.Obj);

            var cuisines = frequentlyOrderedRestaurants
                .Select(r => r.CuisineType)
                .Distinct();

            return await _context.Restaurant
                .Except(frequentlyOrderedRestaurants)
                .Where(r => cuisines.Contains(r.CuisineType)) // TODO: reimplement using Select on the cuisines var, and a seoncary index on r.CuisineType
                .Take(10) // TODO: class variable?
                .ToListAsync();
        }


        public async Task<RecommendationResult<Product>> RecommendProducts(string userId)
        {
            return new RecommendationResult<Product>
            {
                Similar = await GetSimilarProducts(userId),
                Frequented = await _orderRepo.GetFrequentlyOrderedProducts(userId)
            };
        }


        public async Task<List<Product>> GetSimilarProducts(string userId)
        {
            var frequentlyOrderedProductsDto = await _orderRepo.GetFrequentlyOrderedProducts(userId);
            var frequentlyOrderedProducts = frequentlyOrderedProductsDto.Select(dto => dto.Obj);

            var productCategories = frequentlyOrderedProducts
                .Select(p => p.Category)
                .Distinct();

            return await _context.Product
                .Except(frequentlyOrderedProducts) // New products
                .Where(p => productCategories.Contains(p.Category))
                .Take(20)
                .ToListAsync();
        }



    }
}