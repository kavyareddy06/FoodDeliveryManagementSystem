using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class OrderTrackingRepository:IOrderTrackingRepository
    {
        private readonly FoodDeliveryContext _context;

        public OrderTrackingRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderTracking>> GetOrderTrackingAsync(int orderId)
        {
            return await _context.OrderTrackings
                                 .Where(ot => ot.OrderId == orderId)
                                 .OrderByDescending(ot => ot.Timestamp) // Order by latest first
                                 .ToListAsync();
        }

        public async Task<OrderTracking> GetLatestTrackingAsync(int orderId)
        {
            return await _context.OrderTrackings
                                 .Where(ot => ot.OrderId == orderId)
                                 .OrderByDescending(ot => ot.Timestamp)
                                 .FirstOrDefaultAsync();
        }

        public async Task AddTrackingAsync(OrderTracking orderTracking)
        {
            await _context.OrderTrackings.AddAsync(orderTracking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateTrackingStatusAsync(int trackingId, string status, string location)
        {
            var tracking = await _context.OrderTrackings.FindAsync(trackingId);
            if (tracking == null)
                return false;

            tracking.Status = status;
            tracking.Location = location;
            tracking.Timestamp = DateTime.UtcNow;

            _context.OrderTrackings.Update(tracking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
