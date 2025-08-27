using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace E_commerceApplication.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userManagerMock = IdentityMocks.MockUserManager<ApplicationUser>();
            _userService = new UserService(_userManagerMock.Object);
        }

        [Fact]
        public async Task GetProfileAsync_ShouldReturnUserProfile_WhenUserExists()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "test@gmail.com",
                PhoneNumber = "+35987644321",
                AddressDelivery = "Somewhere"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id.ToString()))
                            .ReturnsAsync(user);

            var result = await _userService
                .GetProfileAsync(user.Id.ToString());

            Assert.NotNull(result);
            Assert.Equal(user.UserName, result!.UserName);
            Assert.Equal(user.PhoneNumber, result.PhoneNumber);
            Assert.Equal(user.AddressDelivery, result.AddressDelivery);
        }

        [Fact]
        public async Task GetProfileAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            Guid userId = Guid.NewGuid();

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _userService
                .GetProfileAsync(userId.ToString());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_UserExists_UpdatesProfile()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "oldName" };
            string username = "newName";
            string phoneNumber = "+123987654231";
            string addressDelivery = "New Address";

            var updateModel = new UserProfileModel
            {
                UserName = "newName",
                PhoneNumber = "+123987654231",
                AddressDelivery = "New Address"
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(user.Id.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userService.UpdateUserProfileAsync(user.Id.ToString(), updateModel);

            Assert.True(result.Succeeded);
            Assert.Equal(username, user.UserName);
            Assert.Equal(phoneNumber, user.PhoneNumber);
            Assert.Equal(addressDelivery, user.AddressDelivery);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
        {
            var userId = Guid.NewGuid();
            string username = "newName";
            string phoneNumber = "+123987654231";
            string addressDelivery = "New Address";

            var updateModel = new UserProfileModel
            {
                UserName = username,
                PhoneNumber = phoneNumber,
                AddressDelivery = addressDelivery
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.UpdateUserProfileAsync(userId.ToString(), updateModel));
        }

        [Fact]
        public async Task UpdatePasswordAsync_UserExists_ChangesPassword()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "testUser" };
            string currentPassword = "oldPass";
            string newPassword = "newPass";

            var passwordModel = new UpdatePasswordModel
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(user.Id.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.ChangePasswordAsync(user, currentPassword, newPassword))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userService.UpdatePasswordAsync(user.Id.ToString(), passwordModel);

            Assert.True(result.Succeeded);
        }
        
        [Fact]
        public async Task UpdatePasswordAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
        {
            var userId = Guid.NewGuid();
            string currentPassword = "oldPass";
            string newPassword = "newPass";

            var passwordModel = new UpdatePasswordModel
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.UpdatePasswordAsync(userId.ToString(), passwordModel));
        }
    }
}
