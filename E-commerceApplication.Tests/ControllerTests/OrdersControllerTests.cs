using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Controllers;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using E_commerceApplication.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace E_commerceApplication.Tests.ControllerTests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrdersService> _ordersServiceMock;
        private readonly OrdersController _controller;
        private readonly Guid _userId;

        public OrdersControllerTests()
        {
            _ordersServiceMock = new Mock<IOrdersService>();
            _controller = new OrdersController(_ordersServiceMock.Object);
            _userId = Guid.NewGuid();

            ControllerTestHelper
                .SetUser(_controller, _userId);
        }

        [Fact]
        public async Task AddToOrder_ReturnsCreatedAtAction_WhenValid()
        {
            int productId1 = 1;
            int amount = 2;

            var dto = new AddOrderRequestDto { ProductId = productId1, Amount = amount };

            _ordersServiceMock
                .Setup(s => s.CheckGameByIdAsync(dto.ProductId))
                .ReturnsAsync(true);

            var result = await _controller
                .AddToOrder(dto);

            var createdAtAction = Assert
                .IsType<CreatedAtActionResult>(result);

            Assert
                .Equal(nameof(OrdersController.LoadOrder), createdAtAction.ActionName);
        }

        [Fact]
        public async Task AddToOrder_ReturnsBadRequest_WhenInvalidProduct()
        {
            int invalidProductId = 99;
            int amount = 1;

            var dto = new AddOrderRequestDto { ProductId = invalidProductId, Amount = amount };

            _ordersServiceMock
                .Setup(s => s.CheckGameByIdAsync(dto.ProductId))
                .ReturnsAsync(false);

            var result = await _controller
                .AddToOrder(dto);

            var badRequest = Assert
                .IsType<BadRequestObjectResult>(result);

            Assert
                .Equal(OrderControllerFailedActionsMessages.CreateOrderBadRequest, badRequest.Value);
        }

        [Fact]
        public async Task LoadOrder_ReturnsEmptyList_WhenNoPendingOrder()
        {
            _ordersServiceMock
                .Setup(s => s.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync((Order?)null);

            var result = await _controller
                .LoadOrder();

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var list = Assert
                .IsAssignableFrom<List<LoadOrderItemModel>>(okResult.Value);

            Assert
                .Empty(list);
        }

        [Fact]
        public async Task LoadOrder_ReturnsOrderItems_WhenOrderExists()
        {
            int orderId = 10;
            int orderItemId = 1;
            int productId = 5;
            int amount = 2;

            var order = new Order { Id = orderId, UserId = _userId };

            var items = new List<LoadOrderItemModel>
            {
                new() { OrderItemId = orderItemId, ProductId = productId, Amount = amount }
            };

            _ordersServiceMock
                .Setup(s => s.GetOrderByUserIdAsync(_userId))
                .ReturnsAsync(order);

            _ordersServiceMock
                .Setup(s => s.GetLoadItemListByOrderIdAsync(order.Id))
                .ReturnsAsync(items);

            var result = await _controller
                .LoadOrder();

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var value = Assert
                .IsAssignableFrom<List<LoadOrderItemModel>>(okResult.Value);

            Assert
                .Single(value);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsOk_WhenUpdated()
        {
            int orderItemId = 1;
            int amount = 5;
            int firstOrderItemIndex = 0;

            var dto = new UpdateOrderDto
            {
                Items = new List<UpdateOrderItemDto>
                {
                    new() { OrderItemId = orderItemId, Amount = amount }
                }
            };

            var updatedItems = new List<OrderItem>
            {
                new() { Id = orderItemId, Amount = amount }
            };

            _ordersServiceMock
                .Setup(s => s.UpdateOrderAsync(It.IsAny<UpdateOrderModel>()))
                .ReturnsAsync(updatedItems);

            var result = await _controller
                .UpdateOrder(dto);

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var response = Assert
                .IsType<UpdateOrderItemListModel>(okResult.Value);

            Assert
                .Single(response.updateOrderItemListModel);

            Assert.Equal(amount, response.updateOrderItemListModel[firstOrderItemIndex].Amount);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsBadRequest_WhenServiceReturnsNull()
        {
            int orderItemId = 1;
            int amount = 5;

            var dto = new UpdateOrderDto
            {
                Items = new List<UpdateOrderItemDto>
                {
                    new() { OrderItemId = orderItemId, Amount = amount }
                }
            };

            _ordersServiceMock
                .Setup(s => s.UpdateOrderAsync(It.IsAny<UpdateOrderModel>()))
                .ReturnsAsync((List<OrderItem>?)null);

            var result = await _controller
                .UpdateOrder(dto);

            var badRequest = Assert
                .IsType<BadRequestObjectResult>(result);

            Assert
                .Equal(OrderControllerFailedActionsMessages.UpdateOrderBadRequest, badRequest.Value);
        }

        [Fact]
        public async Task RemoveOrderItems_ReturnsNoContent_WhenSuccess()
        {
            int orderItem1 = 1;
            int orderItem2 = 2;

            var dto = new DeleteItemsDto { ItemsIdList = new List<int> { orderItem1, orderItem2 } };

            _ordersServiceMock
                .Setup(s => s.DeleteItemsFromOrderAsync(It.IsAny<DeleteItemsModel>()))
                .ReturnsAsync(true);

            var result = await _controller
                .RemoveOrderItems(dto);

            Assert
                .IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveOrderItems_ReturnsBadRequest_WhenFailure()
        {
            int orderItem1 = 1;
            int orderItem2 = 2;

            var dto = new DeleteItemsDto { ItemsIdList = new List<int> { orderItem1, orderItem2 } };

            _ordersServiceMock
                .Setup(s => s.DeleteItemsFromOrderAsync(It.IsAny<DeleteItemsModel>()))
                .ReturnsAsync(false);

            var result = await _controller
                .RemoveOrderItems(dto);

            var badRequest = Assert
                .IsType<BadRequestObjectResult>(result);

            Assert
                .Equal(OrderControllerFailedActionsMessages.RemoveOrderItemsBadRequest, badRequest.Value);
        }

        [Fact]
        public async Task BuyOrderItems_ReturnsNoContent_WhenSuccess()
        {
            _ordersServiceMock
                .Setup(s => s.BuyOrderItems(_userId))
                .ReturnsAsync(true);

            var result = await _controller
                .BuyOrderItems();

            Assert
                .IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task BuyOrderItems_ReturnsBadRequest_WhenFailure()
        {
            _ordersServiceMock
                .Setup(s => s.BuyOrderItems(_userId))
                .ReturnsAsync(false);

            var result = await _controller
                .BuyOrderItems();

            var badRequest = Assert
                .IsType<BadRequestObjectResult>(result);

            Assert
                .Equal(OrderControllerFailedActionsMessages.BuyOrderItemsBadRequest, badRequest.Value);
        }

    }
}