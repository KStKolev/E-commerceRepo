using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerceApplication.Controllers
{
    /// <summary>
    /// Controller for managing user profiles and passwords.
    /// </summary>
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

        /// <summary>
        /// Retrieves the profile information for the currently authenticated user.
        /// </summary>
        /// <returns>
        /// 200 OK with the profile data, or 404 Not Found if no profile exists.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfileDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Updates the profile information for the currently authenticated user.
        /// </summary>
        /// <param name="userProfileDto">
        /// The new profile data, including username, phone number, and delivery address.
        /// </param>
        /// <returns>
        /// 200 OK with the updated profile, or 400 Bad Request if the update fails due to validation or other errors.
        /// </returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Updates the password for the currently authenticated user.
        /// </summary>
        /// <param name="updatePasswordDto">
        /// New password data, including current and new passwords.
        /// </param>
        /// <returns>
        /// 204 No Content with password update, or 400 Bad Request if the update fails due to validation or other errors.
        /// </returns>
        [HttpPatch("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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