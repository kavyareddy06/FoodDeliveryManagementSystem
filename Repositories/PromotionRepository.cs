using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class PromotionRepository:IPromotionRepository
    {
        private readonly FoodDeliveryContext _context;

        public PromotionRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await _context.Promotions.ToListAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            return await _context.Promotions
                                 .Include(p => p.Restaurant)  // Include related restaurant data
                                 .FirstOrDefaultAsync(p => p.PromotionId == promotionId);
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Promotions
                                 .Where(p => p.RestaurantId == restaurantId)
                                 .ToListAsync();
        }

        public async Task<Promotion> CreatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task<Promotion> UpdatePromotionAsync(Promotion promotion)
        {
            // Check if the current date is past the end date
            if (promotion.EndDate < DateTime.Now)
            {
                promotion.Status = "expired";  // Set status to 'expired'
            }

            // Update the promotion in the database
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();

            return promotion;
        }


        public async Task<bool> DeletePromotionAsync(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion == null)
            {
                return false;
            }

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Promotion> GetPromotionByCodeAsync(string code)
        {
            return await _context.Promotions
                                 .FirstOrDefaultAsync(p => p.Code == code && p.Status == "active" && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now);
        }
    }
}
