using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Extensions;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Repository;
using OnlineFoodDelivery.Services;

namespace OnlineFoodDelivery.Controllers
{

    [Route("/api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepo;
        private readonly IRecommendationService _recommendationService;
        private readonly IAccountRepository _accountRepo;


        public OrderController(
            IOrderService orderService,
            IOrderRepository orderRepo,
            IRecommendationService recommendationService,
            IAccountRepository accountRepo)
        {
            _orderService = orderService;
            _orderRepo = orderRepo;
            _recommendationService = recommendationService;
            _accountRepo = accountRepo;
        }


        [HttpPost("email/{id:int}")]
        [Authorize(Policy = "OwnsOrder")]
        public async Task<IActionResult> EmailOrderReciept([FromRoute] int id)
        {
            var reciept = await _orderService.EmailReciept(id);

            if (reciept == null)
                return BadRequest();

            return Ok(reciept);
        }

        [HttpPost("reorder/{id:int}")]
        [Authorize(Policy = "OwnsOrder")]
        public async Task<IActionResult> ReOrderFromPrevious([FromRoute] int id, [FromBody] CreateOrderDto orderDto)
        {
            var order = await _orderService.ReOrderFromPrevious(id, orderDto);

            if (order == null)
                return BadRequest();

            return Ok(order);
        }

        [HttpGet("recommend")]
        [Authorize]
        public async Task<IActionResult> GetProductRecommendations()
        {
            var userId = User.GetUserId();
            var recommendations = await _recommendationService.GetUserRecommendations(userId);

            return Ok(recommendations);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto orderDto)
        {
            // Take items from user's basket, apply discount and assign driver
            var userId = User.GetUserId();
            var basketItems = await _accountRepo.GetBasket(userId);
            var order = await _orderService.CreateOrderFromItems(userId, orderDto, basketItems);

            if (order == null)
                return BadRequest();

            return Ok(order);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepo.GetAllAsync();

            return Ok(orders);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetAllByUser()
        {
            var userId = User.GetUserId();
            var orders = await _orderRepo.GetAllAsync(userId);

            return Ok(orders);
        }

        [HttpGet("user/product")]
        [Authorize]
        public async Task<IActionResult> GetOrderedProducts()
        {
            var userId = User.GetUserId();
            var products = await _orderRepo.GetOrderedProducts(userId);

            return Ok(products);
        }

        [HttpGet("user/product/frequent")]
        [Authorize]
        public async Task<IActionResult> GetFrequentlyOrderedProducts()
        {
            var userId = User.GetUserId();
            var products = await _orderRepo.GetFrequentlyOrderedProducts(userId);

            return Ok(products);
        }

        [HttpGet("user/restaurant")]
        [Authorize]
        public async Task<IActionResult> GetOrderedRestaurants()
        {
            var userId = User.GetUserId();
            var restaurants = await _orderRepo.GetOrderedRestaurants(userId);

            return Ok(restaurants);
        }

        [HttpGet("user/restaurant/frequent")]
        [Authorize]
        public async Task<IActionResult> GetFrequentlyOrderedRestaurants()
        {
            var userId = User.GetUserId();
            var restaurants = await _orderRepo.GetFrequentlyOrderedRestaurants(userId);

            return Ok(restaurants);
        }



        /* Get order status */
        [HttpGet("{id:int}")]
        [Authorize(Policy = "OwnsOrder")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderRepo.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Policy = "OwnsOrder")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateOrderDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var checkOrder = await _orderRepo.GetByIdAsync(id);

            if (checkOrder == null)
                return NotFound();

            var order = await _orderRepo.UpdateAsync(id, updateDto);

            return Ok(order);
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Policy = "OwnsOrder")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderRepo.DeleteAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}