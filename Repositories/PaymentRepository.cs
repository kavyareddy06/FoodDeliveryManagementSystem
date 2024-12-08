using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly FoodDeliveryContext _context;

        public PaymentRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        // Create a new payment record
        public async Task<int> CreatePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment.PaymentId;  // Return the newly created payment ID
        }

        // Get payment details by OrderId
        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                                 .Where(p => p.OrderId == orderId)
                                 .FirstOrDefaultAsync();
        }

        // Update payment status
        public async Task UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment != null)
            {
                payment.PaymentStatus = status;
                await _context.SaveChangesAsync();
            }
        }
        public void UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
