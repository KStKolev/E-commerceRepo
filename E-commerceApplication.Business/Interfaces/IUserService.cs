using E_commerceApplication.Business.Models;
using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication.Business.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> UpdatePasswordAsync(string userId, UpdatePasswordModel updatePasswordModel);
        Task<IdentityResult> UpdateUserProfileAsync(string userId, UserProfileModel userProfileDto);
        Task<UserProfileModel?> GetProfileAsync(string userId);
    }
}
