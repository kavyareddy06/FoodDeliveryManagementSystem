using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IUserService _userService;
        public DeliveryController(IDeliveryService deliveryService,IUserService userService)
        {
            _deliveryService = deliveryService;
            _userService = userService;
        }

        // Get delivery information by OrderId
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetDeliveryByOrderId(int orderId)
        {
            var delivery = await _deliveryService.GetDeliveryByOrderIdAsync(orderId);
            if (delivery == null)
            {
                return NotFound(new { message = "Delivery information not found for this order." });
            }
            return Ok(delivery);
        }

        // Get all deliveries (for admin or managers)
        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries);
        }
        [Authorize(Roles = "RestaurantOwner,Admin")]
        [HttpGet("delivery-drivers")]
        public async Task<IActionResult> GetDeliveryDrivers()
        {
            var deliveryDrivers = await _userService.GetUsersByRoleAsync("DeliveryDriver");

            if (deliveryDrivers == null || !deliveryDrivers.Any())
            {
                return NotFound("No delivery drivers found.");
            }

            return Ok(deliveryDrivers); // Return the list of drivers (ID, Name)
        }


        // Add a new delivery entry
        [HttpPost]
        [Authorize(Roles = "RestaurantOwner,Admin")]
        public async Task<IActionResult> AddDelivery([FromBody] Delivery delivery)
        {
            if (delivery == null)
            {
                return BadRequest(new { message = "Invalid delivery data." });
            }

            await _deliveryService.AddDeliveryAsync(delivery);
            return CreatedAtAction(nameof(GetDeliveryByOrderId), new { orderId = delivery.OrderId }, delivery);
        }

        // Update delivery status (e.g., "Picked Up", "In Transit", "Delivered")
        [HttpPut("{deliveryId}/status")]
        [Authorize(Roles = "DeliveryDriver")]
      //  [HttpPut("{deliveryId}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(int deliveryId, [FromBody] DeliveryStatusUpdate request)
        {
            try
            {
                // Call the async method to update the delivery status
                await _deliveryService.UpdateDeliveryStatusAsync(deliveryId, request.Status, request.Location);

                return Ok(new { message = "Delivery status updated successfully" });
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

    }

    // DTO for updating delivery status
    public class DeliveryStatusUpdate
        {
            public string Status { get; set; }
            public string Location { get; set; }
        }
    
}

