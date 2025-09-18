namespace E_commerceApplication.DAL.Interfaces
{
    public interface IRatingValidationRepository
    {
        Task<bool> CheckProductByIdAsync(int productId);
    }
}
