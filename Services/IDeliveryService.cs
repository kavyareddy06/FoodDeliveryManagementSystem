using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IDeliveryService
    {
        public Task<Delivery> GetDeliveryByOrderIdAsync(int orderId);
        public Task<IEnumerable<Delivery>> GetAllDeliveriesAsync();
        public Task AddDeliveryAsync(Delivery delivery);
        public Task UpdateDeliveryStatusAsync(int deliveryId, string status, string location);
    }
}
