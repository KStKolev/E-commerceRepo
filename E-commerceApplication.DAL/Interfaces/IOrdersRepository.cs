using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.DAL.Interfaces
{
    public interface IOrdersRepository
    {
        Task AddOrderAsync(Order order);

        Task AddOrderItemAsync(OrderItem orderItem);

        Task SaveChangesAsync();

        Task DeleteProductsFromOrderItemAsync(List<OrderItem> orderItemList);

        Task BuyOrderAsync(Order order);

        Task<Order?> GetOrderByUserIdAsync(Guid userId);

        Task<List<OrderItem>> GetOrderItemListByOrderIdAsync(int orderId);

        Task<List<OrderItem>> GetOrderItemListByIdCollectionAsync(List<int> orderItemIdCollection);
    }
}