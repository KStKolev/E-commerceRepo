using E_commerceApplication.DTOs;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.Tests
{
    public class AuthCredentialsRequestDtoTest
    {
        [Fact]
        public void AuthCredentialsRequestDto_Should_Pass_Valid_Data()
        {
            var dto = new AuthCredentialsRequestDto
            {
                Email = "noon123@gmail.com",
                Password = "Pass123@321!"
            };

            var results = ValidateModel(dto);
            Assert.Empty(results);
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Missing_Email()
        {
            var dto = new AuthCredentialsRequestDto
            {
                Email = "",
                Password = "Pass123@321!"
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Email is required"));
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Missing_Password()
        {
            var dto = new AuthCredentialsRequestDto
            {
                Email = "noon123@gmail.com",
                Password = ""
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Password is required"));
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Invalid_Email()
        {
            var dto = new AuthCredentialsRequestDto
            {
                Email = "A.gmail.com",
                Password = "Hello123!"
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Invalid email format"));
        }

        [Fact]
        public void AuthCredentialsRequestDto_Should_Not_Pass_Invalid_Password()
        {
            var dto = new AuthCredentialsRequestDto
            {
                Email = "noon123@gmail.com",
                Password = "A23d!e"
            };

            var results = ValidateModel(dto);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Password must be at least 8 characters"));
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }
    }
}
