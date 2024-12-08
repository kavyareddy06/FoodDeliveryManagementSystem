using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class CartRepository:ICartRepository
    {
        private readonly FoodDeliveryContext _context;
        public CartRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(int userId, int restaurantId)
        {
            return await _context.Carts
                                 .Include(c => c.CartItems)
                                 .ThenInclude(ci => ci.MenuItem)
                                 .FirstOrDefaultAsync(c => c.UserId == userId && c.RestaurantId == restaurantId);
        }

        public async Task<CartItem> GetCartItemAsync(int cartItemId)
        {
            return await _context.CartItems
                                 .Include(ci => ci.MenuItem)
                                 .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }
        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _context.CartItems
                                 .Where(c => c.Cart.UserId == userId)
                                 .Include(c => c.MenuItem) // Include MenuItem data
                                 .ToListAsync();
        }

        public async Task AddToCartAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await GetCartItemAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
        {
            return await _context.CartItems
                                 .Where(ci => ci.CartId == cartId)
                                 .Include(ci => ci.MenuItem)
                                 .ToListAsync();
        }
    }
}

