using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Business.Resources;
using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileModel?> GetProfileAsync(string userId)
        {
            ApplicationUser? user = await _userManager
                .FindByIdAsync(userId);

            if (user == null) 
            {
                return null;
            }

            return new UserProfileModel
            {
                UserName = user.UserName ?? string.Empty,
                AddressDelivery = user.AddressDelivery,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };
        }


        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserProfileModel userProfileDto)
        {
            ApplicationUser? user = await _userManager
                .FindByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            }

            user.UserName = userProfileDto.UserName;
            user.PhoneNumber = userProfileDto.PhoneNumber;
            user.AddressDelivery = userProfileDto.AddressDelivery;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdatePasswordAsync(string userId, UpdatePasswordModel updatePasswordModel)
        {
            ApplicationUser? user = await _userManager
                .FindByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            }

            return await _userManager
                .ChangePasswordAsync(user, updatePasswordModel.CurrentPassword, updatePasswordModel.NewPassword);
        }
    }
}
