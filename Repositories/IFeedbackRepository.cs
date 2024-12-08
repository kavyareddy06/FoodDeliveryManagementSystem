using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IFeedbackRepository
    {
        Task<Feedback> GetFeedbackByIdAsync(int feedbackId);
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<IEnumerable<Feedback>> GetFeedbacksByOrderIdAsync(int orderId);
        Task AddFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(int feedbackId);
    }
}
