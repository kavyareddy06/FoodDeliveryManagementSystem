using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Services
{
    public class CartService:ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly FoodDeliveryContext _context;

        public CartService(ICartRepository cartRepository, FoodDeliveryContext context)
        {
            _cartRepository = cartRepository;
            _context = context;
        }

        public async Task AddToCartAsync(int userId, int restaurantId, int itemId, int quantity)
        {
            var cart = await _cartRepository.GetCartAsync(userId, restaurantId);
            var menuItem = await _context.MenuItems.FindAsync(itemId);

            if (menuItem == null) throw new Exception("Menu item not found.");

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    RestaurantId = restaurantId,
                    CreatedDate = DateTime.Now
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = await _cartRepository.GetCartItemsAsync(cart.CartId);
            var existingItem = cartItem.FirstOrDefault(ci => ci.ItemId == itemId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = existingItem.Quantity * menuItem.Price;
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ItemId = itemId,
                    Quantity = quantity,
                    Price = menuItem.Price * quantity
                };
                await _cartRepository.AddToCartAsync(newCartItem);
            }
        }
        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartItemsByUserIdAsync(userId);
        }

        public async Task UpdateCartItemAsync(int cartItemId, int newQuantity)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(cartItemId);
            if (cartItem == null) throw new Exception("Cart item not found.");

            var menuItem = await _context.MenuItems.FindAsync(cartItem.ItemId);
            if (menuItem == null) throw new Exception("Menu item not found.");

            cartItem.Quantity = newQuantity;
            cartItem.Price = menuItem.Price * newQuantity;

            await _cartRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            await _cartRepository.RemoveFromCartAsync(cartItemId);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId, int restaurantId)
        {
            var cart = await _cartRepository.GetCartAsync(userId, restaurantId);
            if (cart == null) return new List<CartItem>();

            return await _cartRepository.GetCartItemsAsync(cart.CartId);
        }
        public async Task<bool> ClearCartAsync(int userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.Cart.UserId == userId )
                .ToListAsync();
            if (cartItems == null || cartItems.Count == 0)
            {
                return false;  // No items in the cart
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
