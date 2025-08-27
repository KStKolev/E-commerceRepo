using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.Tests
{
    public static class ValidationHelper
    {
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }
    }
}
