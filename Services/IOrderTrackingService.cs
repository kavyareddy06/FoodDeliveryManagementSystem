using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IOrderTrackingService
    {
       public Task<IEnumerable<OrderTracking>> GetOrderTrackingAsync(int orderId);
        public Task<OrderTracking> GetLatestTrackingAsync(int orderId);
        public Task AddTrackingAsync(OrderTracking orderTracking);
        public Task<bool> UpdateTrackingStatusAsync(int trackingId, string status, string location);
    }
}
