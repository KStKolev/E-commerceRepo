using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpRequestModel requestDto)
        {
            ApplicationUser newUser = new()
            {
                UserName = requestDto.Email,
                Email = requestDto.Email
            };

            IdentityResult? result = await _userManager
                .CreateAsync(newUser, requestDto.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            string confirmationLink = $"dummy-link/{newUser.Id}/{token}";
            await _emailService.SendEmailConfirmationAsync(newUser.Email, confirmationLink);

            return IdentityResult.Success;
        }

        public async Task<SignInResult> SignInAsync(SignInRequestModel requestDto)
        {
            ApplicationUser? user = await _userManager
                .FindByEmailAsync(requestDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, requestDto.Password))
            {
                return SignInResult.Failed;
            }

            await _signInManager
                .SignInAsync(user, isPersistent: false);

            return SignInResult.Success;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            ApplicationUser? user = await _userManager
                .FindByIdAsync(userId);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            return await _userManager
                .ConfirmEmailAsync(user, token);
        }
    }
}
