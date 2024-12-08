using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class FeedbackService:IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _feedbackRepository.GetAllFeedbacksAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByOrderIdAsync(int orderId)
        {
            return await _feedbackRepository.GetFeedbacksByOrderIdAsync(orderId);
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.AddFeedbackAsync(feedback);
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.UpdateFeedbackAsync(feedback);
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
        }
    }
}
