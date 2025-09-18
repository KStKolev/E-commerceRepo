namespace E_commerceApplication.DAL.Interfaces
{
    public interface IOrdersValidationRepository
    {
        Task<bool> CheckProductByIdAsync(int productId);
    }
}
