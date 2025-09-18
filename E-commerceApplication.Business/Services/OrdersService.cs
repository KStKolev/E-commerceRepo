using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;

namespace E_commerceApplication.Business.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrdersValidationRepository _validationRepository;

        public OrdersService(IOrdersRepository orderRepository, IOrdersValidationRepository validationRepository)
        {
            _ordersRepository = orderRepository;
            _validationRepository = validationRepository;
        }

        public async Task AddItemToOrderAsync(AddOrderModel model)
        {
            Order? activeOrder = await _ordersRepository
                .GetOrderByUserIdAsync(model.UserId);

            if (activeOrder == null)
            {
                activeOrder = new Order
                {
                    UserId = model.UserId,
                    CreationDate = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Pending,
                };

                await _ordersRepository
                    .AddOrderAsync(activeOrder);
            }

            OrderItem orderItem = new() 
            {
                OrderId = activeOrder.Id,
                ProductId = model.ProductId,
                Amount = model.Amount,
            };

            await _ordersRepository
                .AddOrderItemAsync(orderItem);
        }

        public async Task<List<LoadOrderItemModel>> GetLoadItemListByOrderIdAsync(int orderId)
        {
            List<OrderItem> orderItemList = await _ordersRepository
                .GetOrderItemByListOrderIdAsync(orderId);

            List<LoadOrderItemModel> loadedOrderItems = orderItemList
                .Select(oi => new LoadOrderItemModel()
                {
                    OrderItemId = oi.Id,
                    ProductId = oi.ProductId,
                    Amount = oi.Amount
                })
                .ToList();

            return loadedOrderItems;
        }

        public async Task<List<OrderItem>?> UpdateOrderAsync(UpdateOrderModel updateOrderModel)
        {
            Order? pendingOrder = await _ordersRepository
                .GetOrderByUserIdAsync(updateOrderModel.UserId);

            if (pendingOrder == null) 
            {
                return null;
            }

            List<int> orderItemIdList = updateOrderModel
                .Items
                .Select(i => i.OrderItemId)
                .ToList();

            List<OrderItem> orderItemListToUpdate = await _ordersRepository
                .GetOrderItemListByIdCollectionAsync(orderItemIdList);

            Dictionary<int, int> updateLookupDictionary = updateOrderModel
                .Items
                .ToDictionary(i => i.OrderItemId, i => i.Amount);

            foreach (OrderItem orderItem in orderItemListToUpdate)
            {
                if (updateLookupDictionary.TryGetValue(orderItem.Id, out int newAmount))
                {
                    orderItem.Amount = newAmount;
                }
            }

            await _ordersRepository
                .SaveChangesAsync();

            return orderItemListToUpdate;
        }

        public async Task<bool> DeleteItemsFromOrderAsync(DeleteItemsModel deleteItemsModel)
        {
            Order? order = await _ordersRepository
                .GetOrderByUserIdAsync(deleteItemsModel.UserId);

            if (order == null) 
            {
                return false;
            }

            List<OrderItem> orderItemListToDelete = await _ordersRepository
                .GetOrderItemListByIdCollectionAsync(deleteItemsModel.ItemsIdList);

            await _ordersRepository
                .DeleteProductsFromOrderItemAsync(orderItemListToDelete);

            return true;
        }

        public async Task<bool> BuyOrderItems(Guid userId)
        {
            Order? order = await _ordersRepository
                .GetOrderByUserIdAsync(userId);

            if (order == null)
            {
                return false;
            }

            await _ordersRepository
                .BuyOrderAsync(order);

            return true;
        }

        public async Task<Order?> GetOrderByUserIdAsync(Guid userId)
        {
            return await _ordersRepository
                .GetOrderByUserIdAsync(userId);
        }

        public async Task<bool> CheckGameByIdAsync(int productId)
        {
            return await _validationRepository
                .CheckProductByIdAsync(productId);
        }
    }
}