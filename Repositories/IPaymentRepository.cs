using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IPaymentRepository
    {
      public Task<int> CreatePaymentAsync(Payment payment);
        public Task<Payment> GetPaymentByOrderIdAsync(int orderId);
        public Task UpdatePaymentStatusAsync(int paymentId, string status);
        public void UpdatePayment(Payment payment); // Add this method
        public Task SaveChangesAsync();
    }
}
