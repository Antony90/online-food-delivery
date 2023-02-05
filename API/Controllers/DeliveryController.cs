using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Extensions;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Repository;
using OnlineFoodDelivery.Services;

namespace OnlineFoodDelivery.Controllers
{
    [Route("/api/delivery")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {

        private readonly IDeliveryService _deliveryService;
        private readonly IDeliveryRepository _deliveryRepo;

        public DeliveryController(IDeliveryService deliveryService, IDeliveryRepository deliveryRepo)
        {
            _deliveryService = deliveryService;
            _deliveryRepo = deliveryRepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,DeliveryDriver")]
        public async Task<IActionResult> GetAll()
        {
            List<Delivery> deliveries;

            if (User.IsInRole("Admin"))
            {
                deliveries = await _deliveryRepo.GetAllAsync();
            }
            else // Must be a delivery driver
            {
                deliveries = await _deliveryRepo.GetAllAsync(User.GetUserId());

                if (deliveries == null)
                {
                    return NotFound();
                }
            }

            return Ok(deliveries);
        }

        [HttpPatch("rating/{id:int}")]
        [Authorize(Roles = "User", Policy = "IsDeliveryRecipient")]
        public async Task<IActionResult> SetUserRating([FromRoute] int id, [FromQuery] int val)
        {
            await _deliveryRepo.SetUserRating(id, val);

            return Ok();
        }


        /* Sort delivery drivers by user rating */
        [HttpGet("ordered")]
        public async Task<IActionResult> GetOrdered([FromQuery] decimal minRating)
        {
            var drivers = await _deliveryService.GetDriversDescRating(minRating);

            return Ok(drivers);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "IsDeliveryDriver")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var delivery = await _deliveryRepo.GetByIdAsync(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return Ok(delivery);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryDto deliveryDto)
        {
            var delivery = await _deliveryRepo.CreateAsync(deliveryDto);

            if (delivery == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { id = delivery.Id }, delivery);
        }


        [HttpPatch]
        [Route("{id:int}")]
        [Authorize(Policy = "IsDeliveryDriver")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDeliveryDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var delivery = await _deliveryRepo.UpdateAsync(id, updateDto);

            return Ok(delivery);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var delivery = await _deliveryRepo.DeleteAsync(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return Ok();
        }


    }
}