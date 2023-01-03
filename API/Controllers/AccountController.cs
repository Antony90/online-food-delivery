using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Extensions;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Repository;
using OnlineFoodDelivery.Services;

namespace OnlineFoodDelivery.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;
        private readonly IAccountRepository _accountRepo;

        public AccountController(
            UserManager<User> userManager,
            ITokenService tokenService,
            SignInManager<User> signinManager,
            IAccountRepository accountRepo

        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signinManager;
            _accountRepo = accountRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            IAccountDetails details;
            var userId = User.GetUserId();

            if (User.IsInRole("Admin"))
            {
                details = await _accountRepo.GetAdminAccountDetails(userId);
            }
            else if (User.IsInRole("Partner"))
            {
                details = await _accountRepo.GetPartnerAccountDetails(userId);
            }
            else if (User.IsInRole("DeliveryDriver"))
            {
                details = await _accountRepo.GetDeliveryDriverAccountDetails(userId);
            }
            else if (User.IsInRole("User"))
            {
                details = await _accountRepo.GetUserAccountDetails(userId);
            }
            else
            {
                throw new Exception("User has no role.");
            }

            return Ok(details);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null)
                return Unauthorized("Invalid username");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Username or password incorrect");

            return Ok(
                new UserCreatedDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    LocationLong = registerDto.LocationLong,
                    LocationLat = registerDto.LocationLat
                };
                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new UserCreatedDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }



    }
}