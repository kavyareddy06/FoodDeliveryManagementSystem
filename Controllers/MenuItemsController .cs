using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // POST: api/menuitems (Only accessible by RestaurantOwner)
        [HttpPost]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> CreateMenuItem(MenuItem menuItem)
        {
            if (menuItem.RestaurantId == 0)
            {
                return BadRequest("Restaurant ID is missing or invalid.");
            }
            await _menuItemService.CreateMenuItemAsync(menuItem);

            // Fallback to Ok
            return Ok(new { message = "Menu item created successfully.", data = menuItem });
        }



        // GET: api/menuitems/restaurant/{restaurantId} (Accessible by Admin and RestaurantOwner)
        [HttpGet("restaurant/{restaurantId}")]
        [Authorize(Roles = "Admin,RestaurantOwner,Customer")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItemsByRestaurant(int restaurantId)
        {
            var items = await _menuItemService.GetMenuItemsByRestaurantAsync(restaurantId);
            return Ok(items);
        }

        // PUT: api/menuitems/{id} (Only accessible by RestaurantOwner)
        [HttpPut("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItem menuItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Check if the model state is valid

            // Ensure the ItemId in the URL matches the one in the body
            if (id != menuItem.ItemId)
                return BadRequest("Item ID in URL does not match the Item ID in the request body.");

            // Call the service to update the MenuItem
            var result = await _menuItemService.UpdateMenuItemAsync(menuItem);

            // If the MenuItem was not found or the update failed
            if (!result)
            {
                return NotFound("Menu item not found or update failed.");
            }

            // Return a success response
            return Ok(new {message= "Menu item updated successfully.",updatedItem = menuItem });
        }


        // DELETE: api/menuitems/{id} (Only accessible by RestaurantOwner)
        [HttpDelete("{id}")]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            await _menuItemService.DeleteMenuItemAsync(id);
            return NoContent();
        }
    }
}
