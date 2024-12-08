using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IPaymentService _paymentService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IInventoryService _inventoryService;


        public OrderController(ICartService cartService, IOrderService orderService,
            IUserRepository userRepository, IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository,IPaymentService paymentService,
             IInventoryRepository inventoryRepository,
            IInventoryService inventoryService
            )
        {
            _cartService = cartService;
            _orderService = orderService;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
            _paymentService= paymentService;
            _inventoryRepository= inventoryRepository;
            _inventoryService= inventoryService;
        }

        // Place an Order based on Cart
        [HttpPost("{userId}/{restaurantId}")]
        [Authorize(Roles = "Customer")]
        // Update the PlaceOrder method in your OrderController to update inventory


       
        public async Task<IActionResult> PlaceOrder(int userId, int restaurantId, [FromBody] OrderRequest orderRequest)
        {
            if (string.IsNullOrEmpty(orderRequest.DeliveryAddress) || string.IsNullOrEmpty(orderRequest.PaymentMethod))
            {
                return BadRequest("Delivery Address and Payment Method are required.");
            }

            var cartItems = await _cartService.GetCartItemsAsync(userId, restaurantId);

            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest("No items in the cart to place an order.");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

            if (user == null || restaurant == null)
            {
                return NotFound("User or Restaurant not found.");
            }

            var order = new Order
            {
                UserId = userId,
                RestaurantId = restaurantId,
                User = user,
                Restaurant = restaurant,
                OrderDate = DateTime.Now,
                Status = "pending",
                TotalAmount = cartItems.Sum(c => c.Price * c.Quantity),
                DeliveryAddress = orderRequest.DeliveryAddress,
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in cartItems)
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(cartItem.ItemId);
                if (menuItem == null)
                {
                    return BadRequest($"MenuItem with ID {cartItem.ItemId} not found.");
                }

                order.OrderItems.Add(new OrderItem
                {
                    ItemId = cartItem.ItemId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    MenuItem = menuItem
                });

                // Decrease the inventory here
                await _inventoryService.DecreaseInventoryAsync(cartItem.ItemId, cartItem.Quantity); // Decrease stock for each item in the cart
            }

            await _orderService.CreateOrderAsync(order);

            // Create a payment record
            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = order.TotalAmount,
                PaymentMethod = orderRequest.PaymentMethod,
                PaymentStatus = "Pending",
                PaymentDate = DateTime.Now
            };

            await _paymentService.CreatePaymentAsync(payment);
            await _cartService.ClearCartAsync(userId);

            // Trigger inventory tracking after order is placed
            await _inventoryService.TrackInventoryAsync(); // This will check for low stock and notify the owner

            return Ok(new { message = "Order placed successfully.", orderId = order.OrderId });
        }



        public class OrderRequest
        {
            public string DeliveryAddress { get; set; }
            public string PaymentMethod { get; set; }
            // Property to capture the delivery address from the request body
        }

        // Get orders by user ID (customer)
        [HttpGet("user-orders/{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }

            return Ok(orders);
        }
       

        // Get a specific order by OrderId
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        // Update order status
        [HttpPut("update-status/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Invalid status.");
            }
            var validStatuses = new[] { "Pending", "In Preparation", "Out for Delivery", "Delivered", "Cancelled" };

            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid status value.");
            }

            await _orderService.UpdateOrderStatusAsync(orderId, status);
            return Ok(new { message = "Order status updated successfully." });
        }
        [HttpPut("cancel-order/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found.");

            if (order.Status.ToLower() != "pending" && order.Status.ToLower() != "in preparation")
            {
                return BadRequest("Order cannot be cancelled at this stage.");
            }

            await _orderService.UpdateOrderStatusAsync(orderId, "Cancelled");
            return Ok(new { message = "Order cancelled successfully." });
        }
        [HttpGet("track-order/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> TrackOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found.");

            return Ok(new { status = order.Status });
        }

    }
}
