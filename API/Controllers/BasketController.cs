using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class BasketController : ControllerBase
    {
        private readonly AccountRepository _accountRepo;
        private readonly UserManager<User> _userManager;

        public BasketController(UserManager<User> userManager, AccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = _userManager.GetUserId(User);
            var basket = await _userManager.FindByIdAsync(userId);

            if (basket == null)
                return BadRequest();

            return Ok(basket);

        }

        /* Add or remove an item from the basket */
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(UpdateBasketDto updateBasketDto)
        {
            var newBasket = await _accountRepo.UpdateBasket(User.GetUserId(), updateBasketDto);

            if (newBasket == null)
                return BadRequest();

            return Ok(newBasket);
        }


        [HttpDelete]
        [Route("clear")]
        public async Task<IActionResult> Clear()
        {
            await _accountRepo.ClearBasketAsync(User.GetUserId());

            return Ok();
        }
    }
}