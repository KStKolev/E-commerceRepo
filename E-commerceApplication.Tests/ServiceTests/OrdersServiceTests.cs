using E_commerceApplication.Business.Models;
using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;
using Moq;

namespace E_commerceApplication.Tests.ServiceTests
{
    public class OrdersServiceTests
    {
        private readonly Mock<IOrdersRepository> _ordersRepositoryMock;
        private readonly Mock<IOrdersValidationRepository> _validationRepositoryMock;
        private readonly OrdersService _service;
        private readonly Guid _userId;

        public OrdersServiceTests()
        {
            _ordersRepositoryMock = new Mock<IOrdersRepository>();
            _validationRepositoryMock = new Mock<IOrdersValidationRepository>();
            _service = new OrdersService(_ordersRepositoryMock.Object, _validationRepositoryMock.Object);
            _userId = Guid.NewGuid();
        }

        [Fact]
        public async Task AddItemToOrderAsync_CreatesNewOrder_WhenNoPendingOrder()
        {
            int productId = 1;
            int amount = 2;

            var model = new AddOrderModel { UserId = _userId, ProductId = productId, Amount = amount };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync((Order?)null);

            await _service
                .AddItemToOrderAsync(model);

            _ordersRepositoryMock
                .Verify(r => r.AddOrderAsync(It.Is<Order>(o => o.UserId == _userId)), Times.Once);

            _ordersRepositoryMock
                .Verify(r => r.AddOrderItemAsync(It.Is<OrderItem>
                (oi => oi.ProductId == model.ProductId && oi.Amount == model.Amount)), Times.Once);
        }

        [Fact]
        public async Task AddItemToOrderAsync_AddsToExistingOrder_WhenOrderExists()
        {
            int orderId = 10;
            int productId = 1;
            int amount = 2;

            var existingOrder = new Order { Id = orderId, UserId = _userId };
            var model = new AddOrderModel { UserId = _userId, ProductId = productId, Amount = amount };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync(existingOrder);

            await _service
                .AddItemToOrderAsync(model);

            _ordersRepositoryMock
                .Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Never);

            _ordersRepositoryMock
                .Verify(r => r.AddOrderItemAsync(It.Is<OrderItem>
                (oi => oi.OrderId == existingOrder.Id)), Times.Once);
        }

        [Fact]
        public async Task GetLoadItemListByOrderIdAsync_ReturnsMappedModels()
        {
            int orderId = 5;
            int orderItemId = 1;
            int productId = 10;
            int amount = 2;
            int firstIndex = 0;

            var orderItems = new List<OrderItem>
            {
                new() { Id = orderItemId, ProductId = productId, Amount = amount, OrderId = orderId }
            };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderItemByListOrderIdAsync(orderId))
                .ReturnsAsync(orderItems);

            var result = await _service
                .GetLoadItemListByOrderIdAsync(orderId);

            Assert
                .Single(result);

            Assert
                .Equal(orderItems[firstIndex].Id, result[firstIndex].OrderItemId);

            Assert
                .Equal(orderItems[firstIndex].Amount, result[firstIndex].Amount);
        }

        [Fact]
        public async Task UpdateOrderAsync_ReturnsNull_WhenNoPendingOrder()
        {
            var model = new UpdateOrderModel { UserId = _userId, Items = new List<UpdateOrderItemModel>() };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync((Order?)null);

            var result = await _service
                .UpdateOrderAsync(model);

            Assert
                .Null(result);
        }

        [Fact]
        public async Task UpdateOrderAsync_UpdatesAmounts_WhenItemsExist()
        {
            int orderId = 1;
            int orderItemId = 1;
            int newAmount = 10;
            int oldAmount = 1;

            var model = new UpdateOrderModel
            {
                UserId = _userId,
                Items = new List<UpdateOrderItemModel> { new() { OrderItemId = orderItemId, Amount = newAmount } }
            };

            var pendingOrder = new Order { Id = orderId, UserId = _userId };
            var orderItems = new List<OrderItem> { new() { Id = orderItemId, Amount = oldAmount } };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync(pendingOrder);

            _ordersRepositoryMock
                .Setup(r => r.GetOrderItemListByIdCollectionAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(orderItems);

            var result = await _service
                .UpdateOrderAsync(model);

            var updatedList = Assert
                .IsType<UpdateOrderItemListModel>(result);

            var item = Assert
                .Single(updatedList.updateOrderItemListModel);

            Assert
                .Equal(orderItemId, item.OrderItemId);

            Assert
                .Equal(newAmount, item.Amount);

            _ordersRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteItemsFromOrderAsync_ReturnsFalse_WhenOrderDoesNotExist()
        {
            int orderItemId = 1;

            var deleteModel = new DeleteItemsModel { UserId = _userId, ItemsIdList = new List<int> { orderItemId } };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync((Order?)null);

            var result = await _service
                .DeleteItemsFromOrderAsync(deleteModel);

            Assert
                .False(result);
        }

        [Fact]
        public async Task DeleteItemsFromOrderAsync_DeletesItems_WhenOrderExists()
        {
            int orderItemId = 1;
            int orderId = 1;

            var deleteModel = new DeleteItemsModel { UserId = _userId, ItemsIdList = new List<int> { orderItemId } };
            var order = new Order { Id = orderId, UserId = _userId };
            var items = new List<OrderItem> { new() { Id = orderItemId } };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync(order);

            _ordersRepositoryMock
                .Setup(r => r.GetOrderItemListByIdCollectionAsync(deleteModel.ItemsIdList))
                .ReturnsAsync(items);

            var result = await _service
                .DeleteItemsFromOrderAsync(deleteModel);

            Assert
                .True(result);

            _ordersRepositoryMock
                .Verify(r => r.DeleteProductsFromOrderItemAsync(items), Times.Once);
        }

        [Fact]
        public async Task BuyOrderItems_ReturnsFalse_WhenOrderNotFound()
        {
            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync((Order?)null);

            var result = await _service
                .BuyOrderItems(_userId);

            Assert
                .False(result);
        }

        [Fact]
        public async Task BuyOrderItems_ReturnsTrue_WhenOrderExists()
        {
            int orderId = 1;

            var order = new Order { Id = orderId, UserId = _userId };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync(order);

            var result = await _service
                .BuyOrderItems(_userId);

            Assert
                .True(result);

            _ordersRepositoryMock
                .Verify(r => r.BuyOrderAsync(order), Times.Once);
        }

        [Fact]
        public async Task GetOrderByUserIdAsync_ReturnsOrder()
        {
            int orderId = 1;

            var order = new Order { Id = orderId, UserId = _userId };

            _ordersRepositoryMock
                .Setup(r => r.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync(order);

            var result = await _service
                .GetOrderByUserIdAsync(_userId);

            Assert
                .Equal(order, result);
        }

        [Fact]
        public async Task CheckGameByIdAsync_ReturnsTrue_WhenProductValid()
        {
            int productId = 1;

            _validationRepositoryMock
                .Setup(r => r.CheckProductByIdAsync(productId))
                .ReturnsAsync(true);

            var result = await _service
                .CheckGameByIdAsync(productId);

            Assert
                .True(result);
        }

        [Fact]
        public async Task CheckGameByIdAsync_ReturnsFalse_WhenProductInvalid()
        {
            int invalidProductId = 999;

            _validationRepositoryMock
                .Setup(r => r.CheckProductByIdAsync(invalidProductId))
                .ReturnsAsync(false);

            var result = await _service
                .CheckGameByIdAsync(invalidProductId);

            Assert
                .False(result);
        }
    }
}