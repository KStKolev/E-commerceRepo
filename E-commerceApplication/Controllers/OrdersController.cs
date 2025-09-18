using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using E_commerceApplication.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerceApplication.Controllers
{
    /// <summary>
    /// Controller for handling fetching data related to orders.  
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService orderService)
        {
            _ordersService = orderService;
        }

        /// <summary>
        /// Adds a product to the current user's unpaid order.
        /// </summary>
        /// <param name="addOrderRequestDto">The product and amount details to add to the order item.</param>
        /// <returns>
        /// A CreatedAtAction with the created order item if successful or BadRequest if the product ID is unvalid.  
        /// </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToOrder([FromBody] AddOrderRequestDto addOrderRequestDto)
        {
            string? userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            if (!Guid.TryParse(userId, out Guid result))
            {
                return Unauthorized(OrderControllerFailedActionsMessages.UserNotAuthorized);
            }

            bool isGameValid = await _ordersService
                .CheckGameByIdAsync(addOrderRequestDto.ProductId);

            if (!isGameValid) 
            {
                return BadRequest(OrderControllerFailedActionsMessages.CreateOrderBadRequest);
            }

            AddOrderModel createOrderModel = new()
            {
                Amount = addOrderRequestDto.Amount,
                ProductId = addOrderRequestDto.ProductId,
                UserId = result
            };

            await _ordersService
                .AddItemToOrderAsync(createOrderModel); 

            return CreatedAtAction(nameof(LoadOrder), createOrderModel);
        }

        /// <summary>
        /// Loads the current user's unpaid order and its items.
        /// </summary>
        /// <returns>
        /// An Ok with list of order items if found or empty list
        /// </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LoadOrder()
        {
            string? userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            if (!Guid.TryParse(userId, out Guid result))
            {
                return Unauthorized(OrderControllerFailedActionsMessages.UserNotAuthorized);
            }

            Order? pendingOrder = await _ordersService
                .GetOrderByUserIdAsync(result);

            if (pendingOrder == null)
            {
                return Ok(new List<LoadOrderItemModel>());
            }

            List<LoadOrderItemModel> loadOrderItemList = await _ordersService
                .GetLoadItemListByOrderIdAsync(pendingOrder.Id);

            return Ok(loadOrderItemList);
        }

        /// <summary>
        /// Updates amounts for orderItems in the current user's unpaid order.
        /// </summary>
        /// <param name="updateOrderDto">The collection of order items with updated amounts.</param>
        /// <returns>
        /// An Ok with the updated order items if successful or BadRequest if the update fails.  
        /// </returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDto updateOrderDto)
        {
            string? userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            if (!Guid.TryParse(userId, out Guid result))
            {
                return Unauthorized(OrderControllerFailedActionsMessages.UserNotAuthorized);
            }

            UpdateOrderModel updateOrderModel = new()
            {
                UserId = result,
                Items = updateOrderDto.Items
                .Select(i => new UpdateOrderItemModel
                {
                    OrderItemId = i.OrderItemId,
                    Amount = i.Amount
                })
                .ToList()
            };

            List<OrderItem>? updatedOrderItemList = await _ordersService
                .UpdateOrderAsync(updateOrderModel);

            if (updatedOrderItemList == null)
            {
                return BadRequest(OrderControllerFailedActionsMessages.UpdateOrderBadRequest);
            }

            UpdateOrderItemListModel updateOrderItemListModel = new()
            {
                updateOrderItemListModel = updatedOrderItemList
                    .Select(oi => new UpdateOrderItemModel() 
                    {
                        OrderItemId = oi.Id,
                        Amount = oi.Amount      
                    })
                    .ToList()
            };

            return Ok(updateOrderItemListModel);
        }

        /// <summary>
        /// Removes specific items from the current user's active order.
        /// </summary>
        /// <param name="deleteItemsDto">A list of item IDs to remove from the order.</param>
        /// <returns>
        /// A NoContent if removal is successful or BadRequest if removal fails.  
        /// </returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveOrderItems([FromBody] DeleteItemsDto deleteItemsDto)
        {
            string? userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            if (!Guid.TryParse(userId, out Guid result))
            {
                return Unauthorized(OrderControllerFailedActionsMessages.UserNotAuthorized);
            }

            DeleteItemsModel deleteItemsModel = new()
            {
                ItemsIdList = deleteItemsDto.ItemsIdList,
                UserId = result
            };

            bool isItemsListRemoved = await _ordersService
                .DeleteItemsFromOrderAsync(deleteItemsModel);

            if (!isItemsListRemoved)
            {
                return BadRequest(OrderControllerFailedActionsMessages.RemoveOrderItemsBadRequest);
            }

            return NoContent();
        }

        /// <summary>
        /// Completes the purchase of all items in the current user's active order.
        /// </summary>
        /// <returns>
        /// A NoContent if purchase is successful or BadRequest if purchase fails.  
        /// </returns>
        [Authorize]
        [HttpPost("buy")]
        public async Task<IActionResult> BuyOrderItems()
        {
            string? userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            if (!Guid.TryParse(userId, out Guid result))
            {
                return Unauthorized(OrderControllerFailedActionsMessages.UserNotAuthorized);
            }

            bool isOrderCompleted = await _ordersService
                .BuyOrderItems(result);

            if (!isOrderCompleted)
            {
                return BadRequest(OrderControllerFailedActionsMessages.BuyOrderItemsBadRequest);
            }

            return NoContent();
        }
    }
}