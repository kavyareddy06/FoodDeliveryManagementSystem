using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IFeedbackService
    {
       public Task<Feedback> GetFeedbackByIdAsync(int feedbackId);
        public Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();

        public Task<IEnumerable<Feedback>> GetFeedbacksByOrderIdAsync(int orderId);
        public Task AddFeedbackAsync(Feedback feedback);
        public Task UpdateFeedbackAsync(Feedback feedback);
        public Task DeleteFeedbackAsync(int feedbackId);
    }
}
