using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        // Get all promotions
        [HttpGet]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetAllPromotions()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return Ok(promotions);
        }

        // Get promotion by ID
        [HttpGet("{promotionId}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<Promotion>> GetPromotionById(int promotionId)
        {
            var promotion = await _promotionService.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        }

        // Get promotions by restaurant ID
        [HttpGet("restaurant/{restaurantId}")]
        [Authorize(Roles = "Admin,RestaurantOwner,Customer")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotionsByRestaurantId(int restaurantId)
        {
            var promotions = await _promotionService.GetPromotionsByRestaurantIdAsync(restaurantId);
          
            return Ok(promotions);
        }

        // Create promotion
        [HttpPost]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<Promotion>> CreatePromotion([FromBody] Promotion promotion)
        {
            var createdPromotion = await _promotionService.CreatePromotionAsync(promotion);
            return CreatedAtAction(nameof(GetPromotionById), new { promotionId = createdPromotion.PromotionId }, createdPromotion);
        }

        // Update promotion
        [HttpPut("{promotionId}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<Promotion>> UpdatePromotion(int promotionId, [FromBody] Promotion promotion)
        {
            if (promotionId != promotion.PromotionId)
            {
                return BadRequest();
            }

            var updatedPromotion = await _promotionService.UpdatePromotionAsync(promotion);
            if (updatedPromotion == null)
            {
                return NotFound();
            }

            return Ok(updatedPromotion);
        }

        // Delete promotion
        [HttpDelete("{promotionId}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult> DeletePromotion(int promotionId)
        {
            var deleted = await _promotionService.DeletePromotionAsync(promotionId);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
       // [HttpPost("apply-discount")]
        [HttpPost("apply-discount")]
        public async Task<IActionResult> ApplyDiscount([FromBody] DiscountRequest request)
        {
            if (string.IsNullOrEmpty(request?.Code) || request.OrderTotal <= 0)
            {
                return BadRequest("Invalid input.");
            }

            try
            {
                var discountedTotal = await _promotionService.ApplyDiscountAsync(request.Code, request.OrderTotal);
                return Ok(new { discountedTotal });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class DiscountRequest
        {
            public string Code { get; set; }
            public double OrderTotal { get; set; }
        }

    }
}
