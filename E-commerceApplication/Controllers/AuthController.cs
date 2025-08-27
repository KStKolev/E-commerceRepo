using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto requestDto)
        {
            SignUpRequestModel signUpRequestModel = new()
            {
                Email = requestDto.Email,
                Password = requestDto.Password
            };

            var result = await _authService.SignUpAsync(signUpRequestModel);

            if (!result.Succeeded)
            {
                string? errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            return Created();
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
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