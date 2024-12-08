using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IDeliveryRepository
    {
      public  Task<Delivery> GetDeliveryByOrderIdAsync(int orderId);
        public Task<IEnumerable<Delivery>> GetAllDeliveriesAsync();
        public Task<Delivery> GetDeliveryByIdAsync(int deliveryId);
        public Task AddDeliveryAsync(Delivery delivery);
        public Task UpdateDeliveryStatusAsync(int deliveryId, string status, string location);
    }
}
