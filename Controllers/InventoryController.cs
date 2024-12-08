using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("low-stock/{restaurantId}")]
        public IActionResult GetLowStockItemsBasedOnOrders(int restaurantId)
        {
            var lowStockItems = _inventoryService.GetLowStockItemsBasedOnOrders(restaurantId);
            if (lowStockItems == null || !lowStockItems.Any())
            {
                return NotFound("No low-stock items found.");
            }
            return Ok(lowStockItems);
        }

        [HttpPost("track-inventory")]
        public async Task<IActionResult> TrackInventory()
        {
            await _inventoryService.TrackInventoryAsync();
            return Ok(new { Message = "Inventory tracked successfully" });
        }
        [HttpPut("update/{restaurantId}/{itemId}")]
        public async Task<IActionResult> UpdateInventoryQuantity(int restaurantId, int itemId, [FromBody] int newQuantity)
        {
            try
            {
                // Update the inventory quantity
                await _inventoryService.UpdateInventoryQuantityAsync(restaurantId, itemId, newQuantity);

                // Return success response
                return Ok(new { Message = "Inventory updated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                // Handle case when the item is not found in the inventory
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return StatusCode(500, new { Message = "An error occurred while updating the inventory.", Details = ex.Message });
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
        {
            if (inventory == null)
            {
                return BadRequest("Invalid inventory data.");
            }

            // Validate the inventory data
            if (inventory.Quantity <= 0 || inventory.ReorderLevel <= 0)
            {
                return BadRequest("Quantity and Reorder Level should be greater than zero.");
            }

            try
            {
                // Call service to add inventory
                var result = await _inventoryService.AddInventoryAsync(inventory);

                return CreatedAtAction(nameof(GetInventoryById), new { id = result.InventoryId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding inventory: {ex.Message}");
            }
        }

        // Example: Get Inventory by Id (for testing or validation)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            var inventory = await _inventoryService.GetByIdAsync(id);

            if (inventory == null)
            {
                return NotFound("Inventory item not found.");
            }

            return Ok(inventory);
        }
        [HttpGet("all/{restaurantId}")]
        public async Task<IActionResult> GetAllInventoryForRestaurant(int restaurantId)
        {
            var inventories = await _inventoryService.GetInventoryByRestaurantIdAsync(restaurantId);
            if (inventories == null || !inventories.Any())
            {
                return NotFound("No inventory items found for this restaurant.");
            }

            return Ok(inventories);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllInventories()
        {
            var inventories = await _inventoryService.GetAllInventories(); // Await the task to get the result
            if (inventories == null || !inventories.Any()) // Now you can safely check if the list is empty
            {
                return NotFound("No inventories found.");
            }
            return Ok(inventories);
        }


    }
}
