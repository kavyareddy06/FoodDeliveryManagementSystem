using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class DeliveryRepository:IDeliveryRepository
    {
        private readonly FoodDeliveryContext _context;

        public DeliveryRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<Delivery> GetDeliveryByOrderIdAsync(int orderId)
        {
            return await _context.Deliveries
                .Where(d => d.OrderId == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            return await _context.Deliveries
                .Include(d => d.User)  // Include Driver details
                .Include(d => d.Order)  // Include Order details
                .ToListAsync();
        }

        public async Task<Delivery> GetDeliveryByIdAsync(int deliveryId)
        {
            return await _context.Deliveries
                .Include(d => d.User)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
        }

        public async Task AddDeliveryAsync(Delivery delivery)
        {
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeliveryStatusAsync(int deliveryId, string status, string location)
        {
            var delivery = await _context.Deliveries.FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
            if (delivery != null)
            {
                delivery.DeliveryStatus = status;
                delivery.DeliveryAddress = location;

                _context.Deliveries.Update(delivery);
                var changes = await _context.SaveChangesAsync();

                if (changes > 0)
                {
                    Console.WriteLine($"Successfully updated delivery {deliveryId}.");
                }
                else
                {
                    Console.WriteLine($"No changes were made to delivery {deliveryId}.");
                }
            }
            else
            {
                Console.WriteLine($"Delivery with ID {deliveryId} not found.");
            }
        }

    }
}
