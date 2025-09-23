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
                .GetOrderItemListByOrderIdAsync(orderId);

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

        public async Task<UpdateOrderItemListModel?> UpdateOrderAsync(UpdateOrderModel model)
        {
            Order? pendingOrder = await _ordersRepository
                .GetOrderByUserIdAsync(model.UserId);

            if (pendingOrder == null)
            {
                return null;
            }

            Dictionary<int, int> orderItemUpdates = model.Items
                .ToDictionary(i => i.OrderItemId, i => i.Amount);

            List<OrderItem> orderItemsToUpdate = await _ordersRepository
                .GetOrderItemListByIdCollectionAsync(orderItemUpdates.Keys
                .ToList());

            foreach (OrderItem orderItem in orderItemsToUpdate)
            {
                if (orderItemUpdates.TryGetValue(orderItem.Id, out int newAmount))
                {
                    orderItem.Amount = newAmount;
                }
            }

            await _ordersRepository
                .SaveChangesAsync();

            return new UpdateOrderItemListModel
            {
                updateOrderItemListModel = orderItemsToUpdate
                    .Select(oi => new UpdateOrderItemModel
                    {
                        OrderItemId = oi.Id,
                        Amount = oi.Amount
                    })
                    .ToList()
            };
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