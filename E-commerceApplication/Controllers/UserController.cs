using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile() 
        {
            var userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var profile = await _userService
                .GetProfileAsync(userId);

            if (profile == null) 
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UserProfileDto userProfileDto)
        {
            var userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            UserProfileModel userProfileModel = new()
            {
                UserName = userProfileDto.UserName,
                PhoneNumber = userProfileDto.PhoneNumber,
                AddressDelivery = userProfileDto.AddressDelivery
            };  

            var result = await _userService
                .UpdateUserProfileAsync(userId, userProfileModel);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(userProfileModel);
        }

        [HttpPatch("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto) 
        {
            var userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            UpdatePasswordModel updatePasswordModel = new()
            {
                CurrentPassword = updatePasswordDto.CurrentPassword,
                NewPassword = updatePasswordDto.NewPassword
            };

            var result = await _userService
                .UpdatePasswordAsync(userId, updatePasswordModel);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}