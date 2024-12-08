using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(int userId, int restaurantId);
        Task<CartItem> GetCartItemAsync(int cartItemId);
        Task AddToCartAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveFromCartAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        public  Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);

    }
}
