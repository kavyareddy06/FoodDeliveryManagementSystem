using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderReportController : ControllerBase
    {
        private readonly IOrderReportService _orderReportService;
        private readonly FoodDeliveryContext _context;
        private readonly IUserService _userService;

        public OrderReportController(IOrderReportService orderReportService,IUserService userService,FoodDeliveryContext context)
        {
            _orderReportService = orderReportService;
            _userService = userService;
            _context = context;
        }

        // Endpoint to get orders grouped by category
        [HttpGet("grouped-by-category")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrdersGroupedByCategory(
            [FromQuery] int restaurantId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var result = await _orderReportService.GetOrdersGroupedByCategoryAsync(restaurantId, startDate, endDate);
                return Ok(result); // Return the result with HTTP 200 OK
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Handle error and return HTTP 400
            }
        }

        // Endpoint to get orders by restaurant ID
        [HttpGet("by-restaurant-id")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOrdersByRestaurantId(
            [FromQuery] int restaurantId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var result = await _orderReportService.GetOrdersByRestaurantIdAsync(restaurantId, startDate, endDate);
                return Ok(result); // Return the result with HTTP 200 OK
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Handle error and return HTTP 400
            }
        }

        // Endpoint to get orders grouped by date (daily, weekly, monthly)
       // [HttpGet("grouped-by-date")]
        [HttpGet("grouped-by-date")]
        public async Task<IActionResult> GetOrdersGroupedByDate([FromQuery] int? restaurantId, [FromQuery] string frequency, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // Dynamically determine the restaurantId based on the logged-in user
            if (User.IsInRole("RestaurantOwner"))
            {
                var loggedInUser = await _context.Users
                    .Where(u => u.Email == User.Identity.Name && u.Role == "RestaurantOwner")
                    .Join(_context.Restaurants,
                        user => user.UserId,
                        restaurant => restaurant.OwnerId, // Assuming `OwnerUserId` is the column that links Restaurants to Users
                        (user, restaurant) => new { user, restaurant })
                    .Select(x => x.restaurant.RestaurantId)
                    .FirstOrDefaultAsync();

                if (loggedInUser != null)
                {
                    restaurantId = loggedInUser;
                }
                else
                {
                    return BadRequest("Restaurant owner data not found for the logged-in user.");
                }
            }

            // Use the determined or passed `restaurantId` to fetch orders
            if (restaurantId.HasValue)
            {
                var ordersGroupedByDate = await _orderReportService.GetOrdersGroupedByDateAsync(restaurantId.Value, startDate, endDate, frequency);
                return Ok(ordersGroupedByDate);
            }
            else
            {
                return BadRequest("Restaurant ID is required.");
            }

            
        }
    }
}
