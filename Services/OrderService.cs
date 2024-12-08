using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Place an order
        public async Task<int> CreateOrderAsync(Order order)
        {
            // You can add additional logic here like validating the order before saving
            return await _orderRepository.CreateOrderAsync(order);
        }

        // Get all orders for a customer
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        // Get order by OrderId
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        // Update order status
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        }
        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _orderRepository.GetPendingOrdersAsync();
        }
    }
}
