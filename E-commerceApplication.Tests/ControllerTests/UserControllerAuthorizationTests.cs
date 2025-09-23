using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Controllers;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace E_commerceApplication.Tests.ControllerTests
{
    public class UserControllerAuthorizationTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IUserCacheService> _cacheServiceMock;

        public UserControllerAuthorizationTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _cacheServiceMock = new Mock<IUserCacheService>();
            _controller = new UserController(_userServiceMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task GetProfile_ReturnsNotFound_WhenUserNotAuthenticated()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _userServiceMock
            .Setup(s => s.GetProfileAsync(null))
            .ReturnsAsync((UserProfileModel?)null);

            var result = await _controller
                .GetProfile();

            var notFound = Assert
                .IsType<NotFoundResult>(result);

            Assert
                .Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsNullReferenceException_WhenUserNotAuthenticated()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var dto = new UserProfileDto
            {
                UserName = "TestUser",
                PhoneNumber = "12345",
                AddressDelivery = "Somewhere"
            };

            await Assert
                .ThrowsAsync<NullReferenceException>(() => _controller.UpdateProfile(dto));
        }

        [Fact]
        public async Task UpdatePassword_ReturnsNullReferenceException_WhenUserNotAuthenticated()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var dto = new UpdatePasswordDto
            {
                CurrentPassword = "oldpass",
                NewPassword = "newpass123"
            };

            await Assert
                .ThrowsAsync<NullReferenceException>(() => _controller.UpdatePassword(dto));
        }
    }
}