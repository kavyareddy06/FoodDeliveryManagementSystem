using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        // Create a payment for an order
        [HttpPost("make-payment")]
        public async Task<IActionResult> MakePayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest("Invalid payment data.");
            }

            var paymentId = await _paymentService.CreatePaymentAsync(payment);

            if (paymentId == 0)
            {
                return StatusCode(500, "Error processing the payment.");
            }

            return Ok(new { PaymentId = paymentId });
        }

        // Get payment details by OrderId
        [HttpGet("order-payment/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        {
            var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);

            if (payment == null)
            {
                return NotFound("Payment not found for the given order.");
            }

            return Ok(payment);
        }

        // Update payment status
        [HttpPut("update-status/{paymentId}")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentId, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Invalid payment status.");
            }

            await _paymentService.UpdatePaymentStatusAsync(paymentId, status);
            return Ok(new { message = "Payment status updated successfully." });
        }
        [HttpPut("update-payment/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdatePayment(int orderId, [FromBody] PaymentUpdateRequest paymentRequest)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);

            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            // Update payment details
            payment.PaymentMethod = paymentRequest.PaymentMethod;
            payment.PaymentStatus = paymentRequest.PaymentStatus;
            payment.PaymentDate = DateTime.Now;

            await _paymentService.UpdatePaymentAsync(payment);

            // Update order status if payment is successful
            if (payment.PaymentStatus == "Completed")
            {
                order.Status = "In Preparation";
                await _orderService.UpdateOrderStatusAsync(orderId, order.Status);
            }

            return Ok(new { message = "Payment updated successfully." });
        }
        [HttpPost("process-payment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDetails paymentDetails)
        {
            // Handle COD
            if (paymentDetails.PaymentMethod == "COD")
            {
                // Process COD logic (create order, set payment as pending)
                return Ok(new { message = "Order confirmed with Cash on Delivery" });
            }

            // Handle Credit Card payment
            if (paymentDetails.PaymentMethod == "Credit Card")
            {
                // Process credit card (e.g., through a payment gateway)
                // If payment is successful:
                return Ok(new { message = "Payment processed successfully" });
            }

            return BadRequest("Invalid payment method.");
        }

        public class PaymentUpdateRequest
        {
            public string PaymentMethod { get; set; }
            public string PaymentStatus { get; set; }
        }

    }
}
