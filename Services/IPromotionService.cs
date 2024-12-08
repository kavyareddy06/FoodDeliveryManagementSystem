using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IPromotionService
    {
       public Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        public Task<Promotion> GetPromotionByIdAsync(int promotionId);
        public Task<IEnumerable<Promotion>> GetPromotionsByRestaurantIdAsync(int restaurantId);
        public Task<Promotion> CreatePromotionAsync(Promotion promotion);
        public Task<Promotion> UpdatePromotionAsync(Promotion promotion);
        public Task<bool> DeletePromotionAsync(int promotionId);
        public Task<double> ApplyDiscountAsync(string code, double orderTotal);
    }
}
