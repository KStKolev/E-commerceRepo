namespace E_commerceApplication.Validation
{
    public static class ValidationPatterns
    {
        public const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        public const string PhoneNumberPattern = @"^(?=(?:\D*\d){7,15}\D*$)\+?[\d\s().-]+$";
    }
}
