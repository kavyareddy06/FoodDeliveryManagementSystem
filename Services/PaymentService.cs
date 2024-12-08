using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class PaymentService:IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // Process a payment
        public async Task<int> CreatePaymentAsync(Payment payment)
        {
            // You can add payment processing logic here (e.g., integration with payment gateways)
            return await _paymentRepository.CreatePaymentAsync(payment);
        }

        // Get payment details by OrderId
        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
        }

        // Update payment status
        public async Task UpdatePaymentStatusAsync(int paymentId, string status)
        {
            await _paymentRepository.UpdatePaymentStatusAsync(paymentId, status);
        }
        public async Task UpdatePaymentAsync(Payment payment)
        {
            // Validate and update payment record
            _paymentRepository.UpdatePayment(payment);
            await _paymentRepository.SaveChangesAsync();
        }

    }
}
