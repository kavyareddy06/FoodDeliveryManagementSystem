using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class PromotionService:IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await _promotionRepository.GetAllPromotionsAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            return await _promotionRepository.GetPromotionByIdAsync(promotionId);
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByRestaurantIdAsync(int restaurantId)
        {
            return await _promotionRepository.GetPromotionsByRestaurantIdAsync(restaurantId);
        }

        public async Task<Promotion> CreatePromotionAsync(Promotion promotion)
        {
            return await _promotionRepository.CreatePromotionAsync(promotion);
        }

        public async Task<Promotion> UpdatePromotionAsync(Promotion promotion)
        {
            return await _promotionRepository.UpdatePromotionAsync(promotion);
        }

        public async Task<bool> DeletePromotionAsync(int promotionId)
        {
            return await _promotionRepository.DeletePromotionAsync(promotionId);
        }
        public async Task<double> ApplyDiscountAsync(string code, double orderTotal)
        {
            var promotion = await _promotionRepository.GetPromotionByCodeAsync(code);

            if (promotion == null || promotion.Status != "active")
            {
                throw new Exception("Invalid or expired promotion code.");
            }

            if (promotion.StartDate > DateTime.Now || promotion.EndDate < DateTime.Now)
            {
                throw new Exception("Promotion code is not valid for this date.");
            }

            double discountAmount = 0;

            // Convert the discount amount to double for arithmetic operations
            decimal discountAmountDecimal = promotion.DiscountAmount;

            // Apply discount based on type
            if (promotion.DiscountType == "percentage")
            {
                discountAmount = (double)(orderTotal * (double)discountAmountDecimal) / 100;
            }
            else if (promotion.DiscountType == "fixed")
            {
                discountAmount = (double)discountAmountDecimal;
            }

            var discountedTotal = orderTotal - discountAmount;
            return discountedTotal;
        }
        

    }
}
