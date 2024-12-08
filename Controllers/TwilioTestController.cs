using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioTestController : ControllerBase
    {
        private readonly TwilioTestService _twilioTestService;

        public TwilioTestController(TwilioTestService twilioTestService)
        {
            _twilioTestService = twilioTestService;
        }

        [HttpPost("send-test-sms")]
        public IActionResult SendTestSms([FromBody] string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest("Phone number is required.");
            }

            var isSmsSent = _twilioTestService.SendTestSms(phoneNumber);

            if (isSmsSent)
            {
                return Ok("Test SMS sent successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to send test SMS.");
            }
        }
        }
}
