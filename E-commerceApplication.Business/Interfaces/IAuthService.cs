using E_commerceApplication.Business.Models;
using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication.Business.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> SignUpAsync(SignUpRequestModel requestDto);
        Task<SignInResult> SignInAsync(SignInRequestModel requestDto);
        Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token);
    }
}
