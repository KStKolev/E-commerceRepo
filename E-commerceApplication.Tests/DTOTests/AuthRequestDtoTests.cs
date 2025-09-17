using E_commerceApplication.DTOs;

namespace E_commerceApplication.Tests.DTOTests
{
    public class AuthRequestDtoTests
    {
        [Fact]
        public void AuthCredentialsRequestDto_Should_Pass_Valid_Data()
        {
            var dto = new AuthRequestDto
            {
                Email = "noon123@gmail.com",
                Password = "Pass123@321!"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Missing_Email()
        {
            var dto = new AuthRequestDto
            {
                Email = "",
                Password = "Pass123@321!"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Email)));
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Missing_Password()
        {
            var dto = new AuthRequestDto
            {
                Email = "noon123@gmail.com",
                Password = ""
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Password)));
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Invalid_Email()
        {
            var dto = new AuthRequestDto
            {
                Email = "A.gmail.com",
                Password = "Hello123!"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Email)));
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Invalid_Password()
        {
            var dto = new AuthRequestDto
            {
                Email = "noon123@gmail.com",
                Password = "A23d!e"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Password)));
        }
    }
}