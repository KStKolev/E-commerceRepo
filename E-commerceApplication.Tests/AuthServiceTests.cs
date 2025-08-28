using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace E_commerceApplication.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userManagerMock = IdentityMocks.MockUserManager<ApplicationUser>();
            _signInManagerMock = IdentityMocks.MockSignInManager(_userManagerMock);
            _emailServiceMock = new Mock<IEmailService>();

            _authService = new AuthService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _emailServiceMock.Object);
        }

        [Fact]
        public async Task SignUp_ShouldReturnSuccess_WhenUserCreated()
        {
            var request = new SignUpRequestModel { Email = "test@test.com", Password = "Hello123!;" };

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                            .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
                            .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(u => u.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                            .ReturnsAsync("token");

            var result = await _authService.SignUpAsync(request);

            Assert.True(result.Succeeded);
            _emailServiceMock.Verify(e => e.SendEmailConfirmationAsync(request.Email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SignIn_ShouldReturnSuccess_WhenCredentialsValid()
        {
            var request = new SignInRequestModel { Email = "test@test.com", Password = "Hello123!" };
            var user = new ApplicationUser { Email = request.Email };

            _userManagerMock.Setup(u => u.FindByEmailAsync(request.Email))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, request.Password))
                            .ReturnsAsync(true);

            _signInManagerMock.Setup(s => s.SignInAsync(user, false, null))
                              .Returns(Task.CompletedTask);

            var result = await _authService.SignInAsync(request);

            Assert.True(result.Succeeded);
            _signInManagerMock.Verify(s => s.SignInAsync(user, false, null), Times.Once);
        }

        [Fact]
        public async Task EmailConfirm_ShouldReturnSuccess_WhenTokenValid()
        {
            var userId = Guid.NewGuid();
            var token = "token";
            var user = new ApplicationUser { Id = userId };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.ConfirmEmailAsync(user, token))
                            .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.ConfirmEmailAsync(userId, token);

            Assert.True(result.Succeeded);
            _userManagerMock.Verify(u => u.ConfirmEmailAsync(user, token), Times.Once);
        }

        [Fact]
        public async Task SignUp_ShouldFail_WhenUserCreationFails()
        {
            var request = new SignUpRequestModel { Email = "fail@test.com", Password = "Password1" };

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

            var result = await _authService.SignUpAsync(request);

            Assert.False(result.Succeeded);
            Assert.Contains("User creation failed", result.Errors.Select(e => e.Description));
            _emailServiceMock.Verify(e => e.SendEmailConfirmationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SignIn_ShouldFail_WhenUserNotFound()
        {
            var request = new SignInRequestModel { Email = "unknown@test.com", Password = "Password1" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(request.Email))
                            .ReturnsAsync((ApplicationUser?)null);

            var result = await _authService.SignInAsync(request);

            Assert.False(result.Succeeded);
        }


        [Fact]
        public async Task SignIn_ShouldFail_WhenPasswordIncorrect()
        {
            var request = new SignInRequestModel { Email = "test@test.com", Password = "WrongPassword" };
            var user = new ApplicationUser { Email = request.Email };

            _userManagerMock.Setup(u => u.FindByEmailAsync(request.Email))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, request.Password))
                            .ReturnsAsync(false);

            var result = await _authService.SignInAsync(request);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task EmailConfirm_ShouldFail_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            var token = "token";

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                            .ReturnsAsync((ApplicationUser?)null);

            var result = await _authService.ConfirmEmailAsync(userId, token);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task EmailConfirm_ShouldFail_WhenTokenInvalid()
        {
            var userId = Guid.NewGuid();
            var token = "wrong-token";
            var user = new ApplicationUser { Id = userId };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.ConfirmEmailAsync(user, token))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

            var result = await _authService.ConfirmEmailAsync(userId, token);

            Assert.False(result.Succeeded);
            Assert.Contains("Invalid token", result.Errors.Select(e => e.Description));
        }
    }
}