using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class FeedbackRepository:IFeedbackRepository
    {
        private readonly FoodDeliveryContext _context;

        public FeedbackRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _context.Feedbacks
                                 .Include(f => f.Order)
                                 .Include(f => f.User)
                                 .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks
                                 .Include(f => f.Order)
                                 .Include(f => f.User)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByOrderIdAsync(int orderId)
        {
            return await _context.Feedbacks
                                 .Where(f => f.OrderId == orderId)
                                 .Include(f => f.Order)
                                 .Include(f => f.User)
                                 .ToListAsync();
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }
    }
}
