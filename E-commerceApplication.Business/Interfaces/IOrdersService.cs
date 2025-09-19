using E_commerceApplication.Business.Models;
using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.Business.Interfaces
{
    public interface IOrdersService
    {
        Task AddItemToOrderAsync(AddOrderModel model);

        Task<Order?> GetOrderByUserIdAsync(Guid userId);

        Task<List<LoadOrderItemModel>> GetLoadItemListByOrderIdAsync(int orderId);

        Task<UpdateOrderItemListModel?> UpdateOrderAsync(UpdateOrderModel updateOrderModel);

        Task<bool> DeleteItemsFromOrderAsync(DeleteItemsModel deleteItemsModel);

        Task<bool> BuyOrderItems(Guid userId);

        Task<bool> CheckGameByIdAsync(int productId);
    }
}