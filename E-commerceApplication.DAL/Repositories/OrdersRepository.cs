using E_commerceApplication.DAL.Data;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerceApplication.DAL.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context
                .Orders
                .AddAsync(order);

            await _context
                .SaveChangesAsync();
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _context
                .OrderItems
                .AddAsync(orderItem);

            await _context
                .SaveChangesAsync();
        }

        public async Task DeleteProductsFromOrderItemAsync(List<OrderItem> orderItemList)
        {
            _context
                .OrderItems
                .RemoveRange(orderItemList);

            await _context
                .SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context
                .SaveChangesAsync();
        }

        public async Task BuyOrderAsync(Order order)
        {
            order.OrderStatus = OrderStatus.Completed;

            await _context
                .SaveChangesAsync();
        }

        public async Task<Order?> GetOrderByUserIdAsync(Guid userId) 
        {
            return await _context
                .Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == OrderStatus.Pending);
        }

        public async Task<List<OrderItem>> GetOrderItemByListOrderIdAsync(int orderId)
        {
            return await _context
                .OrderItems
                .Where(o => o.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<OrderItem>> GetOrderItemListByIdCollectionAsync(List<int> orderItemIdCollection)
        {
            HashSet<int> orderItemIdHashSet = orderItemIdCollection
                .ToHashSet();

            return await _context
                .OrderItems
                .Where(oi => orderItemIdHashSet.Contains(oi.Id))
                .ToListAsync();
        }
    }
}