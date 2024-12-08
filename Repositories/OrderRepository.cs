using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace FoodDeliveryManagementSystem.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly FoodDeliveryContext _context;

        public OrderRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        // Create a new order
        public async Task<int> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderId;  // Return the newly created order ID
        }

        // Get all orders for a specific customer (UserId)
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                                 .Where(o => o.UserId == userId)
                                 .Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.MenuItem)  // To include the items in the order
                                 .ToListAsync();
        }

        // Get a single order by OrderId
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                                 .Where(o => o.OrderId == orderId)
                                 .Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.MenuItem)
                                 .FirstOrDefaultAsync();
        }

        // Update order status
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == "Pending" || o.Status == "In Preparation" || o.Status == "Out for Delivery")
                .ToListAsync();
        }


    }
}
