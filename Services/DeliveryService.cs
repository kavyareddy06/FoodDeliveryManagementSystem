using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class DeliveryService:IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<Delivery> GetDeliveryByOrderIdAsync(int orderId)
        {
            return await _deliveryRepository.GetDeliveryByOrderIdAsync(orderId);
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            return await _deliveryRepository.GetAllDeliveriesAsync();
        }

        public async Task AddDeliveryAsync(Delivery delivery)
        {
            await _deliveryRepository.AddDeliveryAsync(delivery);
        }

        public async Task UpdateDeliveryStatusAsync(int deliveryId, string status, string location)
        {
            await _deliveryRepository.UpdateDeliveryStatusAsync(deliveryId, status, location);
        }
    }
}
