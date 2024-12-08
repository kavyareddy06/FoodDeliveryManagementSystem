using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly FoodDeliveryContext _context;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        //get cart items
        [HttpGet("{userId}/{restaurantId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(int userId, int restaurantId)
        {
            var items = await _cartService.GetCartItemsAsync(userId, restaurantId);
            if (items == null ||!items.Any())
                return NotFound("No items in the cart.");

            return Ok(items);
        }
        [HttpGet("{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<CartItem>>> GetCartItems(int userId)
        {
            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);

            if (cartItems == null || cartItems.Count == 0)
            {
                return NotFound("Cart is empty or not found.");
            }

            return Ok(cartItems);
        }
        // POST: api/cart/add
        [HttpPost("add")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddToCart(int userId, int restaurantId, int itemId, int quantity)
        {
            try
            {
                await _cartService.AddToCartAsync(userId, restaurantId, itemId, quantity);
                return Ok(new { message = "Item added to cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/cart/update/{cartItemId}
        [HttpPut("update/{cartItemId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, int newQuantity)
        {
            try
            {
                await _cartService.UpdateCartItemAsync(cartItemId, newQuantity);
                return Ok(new { message = "Cart item updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/cart/remove/{cartItemId}
        [HttpDelete("remove/{cartItemId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(cartItemId);
                return Ok(new { message = "Item removed from cart successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart([FromQuery] int userId)
        {
            var result = await _cartService.ClearCartAsync(userId);

            if (result)
            {
                return Ok(new { message = "Cart cleared successfully" });
            }
            else
            {
                return BadRequest(new { message = "No items found in the cart to clear" });
            }
        }

    }
}
