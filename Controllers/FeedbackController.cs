using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }


        // GET: api/feedback/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,RestaurantOwner,Customer")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }

        // GET: api/feedback/order/{orderId}
        [HttpGet("order/{orderId}")]
        [Authorize(Roles = "Admin,RestaurantOwner,Customer")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByOrderId(int orderId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByOrderIdAsync(orderId);

            if (feedbacks == null)
            {
                return NotFound();
            }

            return Ok(feedbacks);
        }

        // POST: api/feedback
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Feedback>> AddFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null)
            {
                return BadRequest("Invalid feedback data.");
            }

            await _feedbackService.AddFeedbackAsync(feedback);

            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.FeedbackId }, feedback);
        }

        // PUT: api/feedback/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] Feedback feedback)
        {
            if (id != feedback.FeedbackId)
            {
                return BadRequest("Feedback ID mismatch.");
            }

            await _feedbackService.UpdateFeedbackAsync(feedback);

            return NoContent();
        }

        // DELETE: api/feedback/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            await _feedbackService.DeleteFeedbackAsync(id);

            return NoContent();
        }
    }
}
