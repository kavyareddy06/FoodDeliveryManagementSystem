using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("sendLowStockEmail")]
        public async Task<IActionResult> SendLowStockEmail([FromBody] LowStockEmailRequest request)
        {
            // Ensure request contains valid data
            if (string.IsNullOrEmpty(request.OwnerEmail) || string.IsNullOrEmpty(request.RestaurantName) ||
                request.CurrentStock < 0 || request.ReorderLevel < 0)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                // Trigger the email
                await _emailService.SendLowStockAlertAsync(
                    request.OwnerEmail,
                    request.RestaurantName,
                    request.ItemName,
                    request.CurrentStock,
                    request.ReorderLevel
                );

                return Ok("Low stock email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }

    // Request model for sending low stock email
    public class LowStockEmailRequest
    {
        public string OwnerEmail { get; set; }
        public string RestaurantName { get; set; }
        public int ItemName { get; set; }
        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }
    }
}

