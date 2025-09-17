using E_commerceApplication.DTOs;

namespace E_commerceApplication.Tests.DTOTests
{
    public class EditRatingDtoTests
    {
        [Fact]
        public void Rating_InvalidInput_ShouldFailValidation() 
        {
            var dto = new EditRatingRequestDto() 
            {
                ProductId = 2,
                Rating = 7
            };

            var results = ValidationHelper
              .ValidateModel(dto);

            Assert
             .Single(results);

            Assert
                .Contains(results, r => r.MemberNames.Contains(nameof(EditRatingRequestDto.Rating)));
        }
    }
}
