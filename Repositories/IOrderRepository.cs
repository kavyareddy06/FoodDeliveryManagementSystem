using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IOrderRepository
    {
       public Task<int> CreateOrderAsync(Order order);
        public Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        public Task<Order> GetOrderByIdAsync(int orderId);
        public Task UpdateOrderStatusAsync(int orderId, string status);
        public Task<IEnumerable<Order>> GetPendingOrdersAsync();
    }
}
