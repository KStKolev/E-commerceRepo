using E_commerceApplication.DTOs;

namespace E_commerceApplication.Tests.DTOTests
{
    public class UserProfileDtoTests
    {
        [Fact]
        public void UserProfileDto_Should_Pass_Valid_Data()
        {
            var dto = new UserProfileDto
            {
                UserName = "Mint",
                PhoneNumber = "+359123123456",
                AddressDelivery = "Sofia, 1000"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }

        [Fact]
        public void UserProfileDto_Should_Not_Pass_Missing_UserName()
        {
            var dto = new UserProfileDto
            {
                UserName = "",
                PhoneNumber = "+359123123456",
                AddressDelivery = "Sofia, 1000"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfileDto.UserName)));
        }

        [Fact]
        public void UserProfileDto_Should_Not_Pass_Missing_Phone_Number()
        {
            var dto = new UserProfileDto
            {
                UserName = "Mint",
                PhoneNumber = "",
                AddressDelivery = "Sofia, 1000"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfileDto.PhoneNumber)));
        }

        [Fact]
        public void UserProfileDto_Should_Not_Pass_Invalid_Phone_Number()
        {
            var dto = new UserProfileDto
            {
                UserName = "Mint",
                PhoneNumber = "12345",
                AddressDelivery = "Sofia, 1000"
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfileDto.PhoneNumber)));
        }

        [Fact]
        public void UserProfileDto_Should_Not_Pass_Missing_AddressDelivery()
        {
            var dto = new UserProfileDto
            {
                UserName = "Mint",
                PhoneNumber = "+359123123456",
                AddressDelivery = ""
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UserProfileDto.AddressDelivery)));
        }
    }
}