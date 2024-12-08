using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class OrderTrackingService:IOrderTrackingService
    {
        private readonly IOrderTrackingRepository _repository;

        public OrderTrackingService(IOrderTrackingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderTracking>> GetOrderTrackingAsync(int orderId)
        {
            return await _repository.GetOrderTrackingAsync(orderId);
        }

        public async Task<OrderTracking> GetLatestTrackingAsync(int orderId)
        {
            return await _repository.GetLatestTrackingAsync(orderId);
        }

        public async Task AddTrackingAsync(OrderTracking orderTracking)
        {
            await _repository.AddTrackingAsync(orderTracking);
        }

        public async Task<bool> UpdateTrackingStatusAsync(int trackingId, string status, string location)
        {
            return await _repository.UpdateTrackingStatusAsync(trackingId, status, location);
        }
    }
}
