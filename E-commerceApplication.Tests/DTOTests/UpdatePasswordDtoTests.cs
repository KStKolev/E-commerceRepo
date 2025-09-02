using E_commerceApplication.DTOs;

namespace E_commerceApplication.Tests.DTOTests
{
    public class UpdatePasswordDtoTests
    {
        [Fact]
        public void UpdatePasswordDto_Should_Update_With_Valid_Data()
        {
            string currentPassword = "OldPass123!";
            string newPassword = "NewPass456!";

            var dto = new UpdatePasswordDto
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
            };

            Assert.Equal(currentPassword, dto.CurrentPassword);
            Assert.Equal(newPassword, dto.NewPassword);
        }

        [Fact]
        public void UpdatePasswordDto_Should_Not_Pass_Missing_CurrentPassword()
        {
            var dto = new UpdatePasswordDto
            {
                CurrentPassword = "",
                NewPassword = "NewPass456!"
            };

            var results = ValidationHelper.ValidateModel(dto);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UpdatePasswordDto.CurrentPassword)));
        }

        [Fact]
        public void UpdatePasswordDto_Should_Not_Pass_Missing_NewPassword()
        {
            var dto = new UpdatePasswordDto
            {
                CurrentPassword = "OldPass123!",
                NewPassword = ""
            };
            var results = ValidationHelper.ValidateModel(dto);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UpdatePasswordDto.NewPassword)));
        }

        [Fact]
        public void UpdatePasswordDto_Should_Not_Pass_Invalid_NewPassword()
        {
            var dto = new UpdatePasswordDto
            {
                CurrentPassword = "OldPass123!",
                NewPassword = "short"
            };
            var results = ValidationHelper.ValidateModel(dto);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UpdatePasswordDto.NewPassword)));
        }
    }
}
