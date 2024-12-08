using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly FoodDeliveryContext _context;

        public RestaurantsController(IRestaurantService restaurantService,FoodDeliveryContext context)
        {
            _restaurantService = restaurantService;
            _context = context;
        }

        // POST: api/restaurants (Only accessible by RestaurantOwner)
        [HttpPost]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<ActionResult<Restaurant>> CreateRestaurant(Restaurant restaurant)
        {
            // Ensure the owner exists by checking the ownerId
            var owner = await _context.Users.FindAsync(restaurant.OwnerId);
            if (owner == null)
            {
                return BadRequest("Owner not found.");
            }

            // If there are menu items, process them
            if (restaurant.MenuItems != null && restaurant.MenuItems.Count > 0)
            {
                foreach (var menuItem in restaurant.MenuItems)
                {
                    menuItem.RestaurantId = restaurant.RestaurantId; // Set the restaurantId for the menu item
                }
            }

            // Add the restaurant to the database
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            // If menu items are provided, add them to the database as well
            if (restaurant.MenuItems != null && restaurant.MenuItems.Count > 0)
            {
                _context.MenuItems.AddRange(restaurant.MenuItems);
                await _context.SaveChangesAsync();
            }

            // Return the created restaurant with the menu items (if provided)
            return CreatedAtAction("GetRestaurant", new { id = restaurant.RestaurantId }, restaurant);
        }


        // GET: api/restaurants/{id} (Accessible by Admin and RestaurantOwner)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(id);
            if (restaurant == null)
                return NotFound();
            return restaurant;
        }
       

        // PUT: api/restaurants/{id} (Only accessible by RestaurantOwner)
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> UpdateRestaurant(int id, Restaurant updatedRestaurant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Check if the model state is valid

            // Call the service to update the restaurant
            var result = await _restaurantService.UpdateRestaurant(id, updatedRestaurant);

            // If the restaurant was not found or the update failed
            if (!result)
            {
                return NotFound("Restaurant not found or update failed.");
            }

            // Return a success response
            return Ok(new { message = "Restaurant updated successfully" });
        }


        // DELETE: api/restaurants/{id} (Only accessible by RestaurantOwner)
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            await _restaurantService.DeleteRestaurantAsync(id);
            return NoContent();
        }
        [HttpGet("search")]
        [Authorize(Roles = "Admin,RestaurantOwner,Customer")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> SearchRestaurants([FromQuery] string restaurant)
        {
            var restaurants = await _restaurantService.SearchRestaurantsAsync(restaurant);
            return Ok(restaurants);
        }

        // GET: api/restaurants/searchbylocation
        [HttpGet("searchbylocation")]
        [Authorize(Roles = "Admin,RestaurantOwner,Customer")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> SearchRestaurantsByLocation([FromQuery] string location)
        {
            var restaurants = await _restaurantService.SearchRestaurantsByLocationAsync(location);
            return Ok(restaurants);
        }
        
        [HttpGet("my-restaurant")]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> GetMyRestaurant()
        {
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" && int.TryParse(c.Value, out _))?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("User ID is not valid or missing.");
            }

            var restaurant = await _restaurantService.GetRestaurantByOwnerId(userId);
            return restaurant != null ? Ok(restaurant) : NotFound("No restaurant found for the logged-in user.");
        }
        [HttpGet()]
        [Authorize(Roles = "Admin,RestaurantOwner,Customer")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetAllRestaurants()
        {
            var restaurants= await _restaurantService.GetAllRestaurantsAsync();
            if (restaurants == null || !restaurants.Any())
            {
                return NotFound(); // Return 404 if no restaurants are found
            }

            return Ok(restaurants);
        }

        [HttpGet("debug-claims")]
        [Authorize]
        public IActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }



    }
}
