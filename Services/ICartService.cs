using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface ICartService
    {
        Task AddToCartAsync(int userId, int restaurantId, int itemId, int quantity);
        Task UpdateCartItemAsync(int cartItemId, int newQuantity);
        Task RemoveFromCartAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId, int restaurantId);
        public Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
       public  Task<bool> ClearCartAsync(int userId);
    }
}
