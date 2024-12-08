using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IPaymentService
    {
        public Task<int> CreatePaymentAsync(Payment payment);
        public Task<Payment> GetPaymentByOrderIdAsync(int orderId);
        public Task UpdatePaymentStatusAsync(int paymentId, string status);
        public Task UpdatePaymentAsync(Payment payment);
    }
}
