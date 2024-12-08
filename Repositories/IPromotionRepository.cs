using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IPromotionRepository
    {
       public Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        public Task<Promotion> GetPromotionByIdAsync(int promotionId);
        public Task<IEnumerable<Promotion>> GetPromotionsByRestaurantIdAsync(int restaurantId);
        public Task<Promotion> CreatePromotionAsync(Promotion promotion);
        public Task<Promotion> UpdatePromotionAsync(Promotion promotion);
        public Task<bool> DeletePromotionAsync(int promotionId);
        public Task<Promotion> GetPromotionByCodeAsync(string code);
    }
}
