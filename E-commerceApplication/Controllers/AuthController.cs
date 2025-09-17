using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceApplication.Controllers
{
    /// <summary>
    /// Controller for authenticating users
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided email and password.
        /// </summary>
        /// <param name="requestDto">
        ///  The sign-up request containing the user's email and password.
        /// </param>
        /// <returns>
        /// Returns 201 Created if the user was successfully registered,  
        /// or 400 Bad Request with error messages if the registration fails. 
        /// </returns>
        [AllowAnonymous]
        [HttpPost("signUp")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto requestDto)
        {
            SignUpRequestModel signUpRequestModel = new()
            {
                Email = requestDto.Email,
                Password = requestDto.Password
            };

            var result = await _authService
                .SignUpAsync(signUpRequestModel);

            if (!result.Succeeded)
            {
                string? errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            return Created();
        }

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="requestDto">
        /// The sign-in request containing the user's email and password.
        /// </param>
        /// <returns>
        /// Returns 200 OK if authentication is successful,  
        /// or 401 Unauthorized if the email or password is invalid.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("signIn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto requestDto) 
        {
            SignInRequestModel signInRequestModel = new()
            {
                Email = requestDto.Email,
                Password = requestDto.Password
            };

            var result = await _authService.SignInAsync(signInRequestModel);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            return Ok();
        }

        /// <summary>
        /// Confirms a user's email address using the provided user ID and confirmation token.
        /// </summary>
        /// <param name="id">
        /// The ID of the user whose email is being confirmed.
        /// </param>
        /// <param name="token">
        /// The email confirmation token sent to the user's email.
        /// </param>
        /// <returns>
        /// Returns 204 No Content if the email was successfully confirmed,  
        /// or 400 Bad Request with error messages if confirmation fails.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("emailConfirm")]
        public async Task<IActionResult> EmailConfirm([FromQuery] string id, [FromQuery] string token)
        {
            var result = await _authService.ConfirmEmailAsync(id, token);

            if (!result.Succeeded)
            {
                string? errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            return NoContent();
        }
    }
}