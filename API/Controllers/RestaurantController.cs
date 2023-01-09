using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Enums;
using OnlineFoodDelivery.Extensions;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Repository;
using OnlineFoodDelivery.Services;

namespace OnlineFoodDelivery.Controllers
{

    [Route("/api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {

        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IRecommendationService _recommendationService;

        public RestaurantController(
            ApplicationDBContext context,
            UserManager<User> userManager,
            IRestaurantRepository restaurantRepo,
            IRecommendationService recommendationService)
        {
            _context = context;
            _userManager = userManager;
            _restaurantRepo = restaurantRepo;
            _recommendationService = recommendationService;
        }

        // Get stores within user's delivery location
        [HttpGet("nearest/{distance:float:min(0)}")]
        [Authorize]
        public async Task<IActionResult> GetNearestWithinRange([FromRoute] float distance)
        {
            var username = User.GetUsername(); // Get from JWT payload
            var user = await _userManager.FindByNameAsync(username);

            Dictionary<int, float> restaurantDistances = [];

            // First cache all of the distances
            await _context.Restaurant.ForEachAsync(r =>
            {
                restaurantDistances.Add(r.Id, Vector2.Distance(r.ToLocationVec(), user.ToLocationVec()));
            });

            // Filter all restaurants within Distance, then sort the filtered list
            var restaurants = await _context.Restaurant.AsNoTracking().ToListAsync();
            var nearestRestaurants = restaurants
                .Where(r => restaurantDistances[r.Id] < distance)
                .OrderByDescending(r => restaurantDistances[r.Id])
                .Reverse();

            return Ok(nearestRestaurants);
        }

        [HttpGet("recommend")]
        [Authorize]
        public async Task<IActionResult> GetRestaurantRecommendations()
        {
            var userId = User.GetUserId();
            var recommendations = await _recommendationService.RecommendRestaurants(userId);

            return Ok(recommendations);
        }


        [HttpGet("cuisine/{cuisineType:int}")]
        public async Task<IActionResult> GetByCuisine([FromRoute] int cuisineType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cuisineTypeEnum = (CuisineType)cuisineType;

            var restaurants = await _restaurantRepo.GetByCuisine(cuisineTypeEnum);

            return Ok(restaurants);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Partner", Policy = "OwnsRestaurant")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRestaurantDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurant = await _restaurantRepo.UpdateAsync(id, updateDto);

            return Ok(restaurant);
        }

        [HttpGet("revenue/{id:int}")]
        [Authorize(Roles = "Partner", Policy = "OwnsRestaurant")]
        public async Task<IActionResult> GetRestaurantRevenue([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var revenue = await _restaurantRepo.GetRestaurantRevenue(id);

            return Ok(revenue);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string searchQuery)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurants = await _restaurantRepo.GetAllAsync();

            var filteredRestaurants = restaurants.Where(r => r.Name.Contains(searchQuery));

            return Ok(filteredRestaurants);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurant = await _context.Restaurant.FindAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantDto createRestaurantDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurant = createRestaurantDto.ToRestaurant();
            await _context.Restaurant.AddAsync(restaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant.ToDto());
        }



    }
}