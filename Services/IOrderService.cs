using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IOrderService
    {
        public Task<int> CreateOrderAsync(Order order);
        public Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        public Task<Order> GetOrderByIdAsync(int orderId);
        public Task UpdateOrderStatusAsync(int orderId, string status);
        public Task<IEnumerable<Order>> GetPendingOrdersAsync();

    }
}
