using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Extensions;
using OnlineFoodDelivery.Repository;
using OnlineFoodDelivery.Services;

namespace OnlineFoodDelivery.Controllers
{

    [Route("/api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IRecommendationService _recommendationService;

        public ProductController(IProductRepository productRepo, IRecommendationService recommendationService)
        {
            _productRepo = productRepo;
            _recommendationService = recommendationService;
        }

        [HttpGet("recommend")]
        [Authorize]
        public async Task<IActionResult> GetProductRecommendations()
        {
            var userId = User.GetUserId();
            var recommendations = await _recommendationService.RecommendProducts(userId);

            return Ok(recommendations);
        }


        [HttpGet("trending")]
        public async Task<IActionResult> GetTrending()
        {
            var products = await _productRepo.GetTrending();

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepo.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
        {
            var product = await _productRepo.CreateAsync(productDto);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepo.UpdateAsync(id, updateDto);

            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepo.DeleteAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}