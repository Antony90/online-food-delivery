using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Services;

namespace OnlineFoodDelivery.Controllers
{
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // register, update admin
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDBContext _context;

        public AdminController(UserManager<User> userManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpDelete("drop")]
        public async Task<IActionResult> DropDatabase()
        {
            var nRows = await _context.Database.ExecuteSqlRawAsync("DROP DATABASE food;");

            return Ok(nRows);
        }
    }
}