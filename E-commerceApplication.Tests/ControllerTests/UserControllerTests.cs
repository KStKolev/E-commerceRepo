using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Controllers;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace E_commerceApplication.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;
        private const string UserId = "user-123";

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task GetProfile_ReturnsOkResult_WhenProfileExists()
        {
            var userProfile = new UserProfileModel
            {
                UserName = "testuser",
                PhoneNumber = "1234567890",
                AddressDelivery = "123 Test St"
            };

            _userServiceMock
                .Setup(s => s.GetProfileAsync(UserId))
                .ReturnsAsync(userProfile);

            var result = await _controller
                .GetProfile();

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var returnValue = Assert
                .IsType<UserProfileModel>(okResult.Value);

            Assert
                .Equal(userProfile.UserName, returnValue.UserName);

            Assert
                .Equal(userProfile.PhoneNumber, returnValue.PhoneNumber);

            Assert
                .Equal(userProfile.AddressDelivery, returnValue.AddressDelivery);
        }

        [Fact]
        public async Task GetProfile_ReturnsNotFound_WhenProfileDoesNotExist()
        {
            _userServiceMock
                .Setup(s => s.GetProfileAsync(UserId))
                    .ReturnsAsync((UserProfileModel?)null);

            var result = await _controller
                .GetProfile();

            Assert
                .IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            var userProfileDto = new UserProfileDto
            {
                UserName = "updateduser",
                PhoneNumber = "0987654321",
                AddressDelivery = "456 Updated St"
            };

            var userProfileModel = new UserProfileModel
            {
                UserName = userProfileDto.UserName,
                PhoneNumber = userProfileDto.PhoneNumber,
                AddressDelivery = userProfileDto.AddressDelivery
            };

            _userServiceMock
                .Setup(s => s.UpdateUserProfileAsync(UserId, It.IsAny<UserProfileModel>()))
                    .ReturnsAsync(IdentityResult.Success);

            var result = await _controller
                .UpdateProfile(userProfileDto);

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var returnValue = Assert
                .IsType<UserProfileModel>(okResult.Value);

            Assert
                .Equal(userProfileModel.UserName, returnValue.UserName);

            Assert
                .Equal(userProfileModel.PhoneNumber, returnValue.PhoneNumber);

            Assert
                .Equal(userProfileModel.AddressDelivery, returnValue.AddressDelivery);
        }

        [Fact]
        public async Task UpdateProfile_ReturnsBadRequest_Failure()
        {
            var dto = new UserProfileDto
            {
                UserName = "Bad",
                PhoneNumber = "000",
                AddressDelivery = "Nowhere"
            };

            string description = "Update failed";

            _userServiceMock
                .Setup(s => s.UpdateUserProfileAsync(UserId, It.IsAny<UserProfileModel>()))
                    .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = description }));

            var result = await _controller
                .UpdateProfile(dto);

            var badRequest = Assert
                .IsType<BadRequestObjectResult>(result);

            var errors = Assert
                .IsAssignableFrom<IEnumerable<IdentityError>>(badRequest.Value);

            Assert
                .Contains(errors, e => e.Description == description);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsNoContent_Success()
        {
            var dto = new UpdatePasswordDto
            {
                CurrentPassword = "old123",
                NewPassword = "new123"
            };

            _userServiceMock
                .Setup(s => s.UpdatePasswordAsync(UserId, It.IsAny<UpdatePasswordModel>()))
                    .ReturnsAsync(IdentityResult.Success);

            var result = await _controller
                .UpdatePassword(dto);

            Assert
                .IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsBadRequest_Failure()
        {
            var dto = new UpdatePasswordDto
            {
                CurrentPassword = "old123",
                NewPassword = "new123"
            };

            string description = "Password update failed";

            _userServiceMock
                .Setup(s => s.UpdatePasswordAsync(UserId, It.IsAny<UpdatePasswordModel>()))
                    .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = description }));

            var result = await _controller
                .UpdatePassword(dto);

            var badRequest = Assert
                .IsType<BadRequestObjectResult>(result);

            var errors = Assert
                .IsAssignableFrom<IEnumerable<IdentityError>>(badRequest.Value);

            Assert
                .Contains(errors, e => e.Description == description);
        }
    }
}