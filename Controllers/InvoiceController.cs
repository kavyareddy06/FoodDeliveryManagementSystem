using FoodDeliveryManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryManagementSystem.Services;
using FoodDeliveryManagementSystem.DataTransferObject;
using FoodDeliveryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly EmailService _emailService;
        private readonly FoodDeliveryContext _foodDeliveryContext;
       // private readonly EmailService _emailService;


        public InvoiceController(InvoiceService invoiceService,EmailService emailService)
        {
            _invoiceService = invoiceService;
            _emailService = emailService;
        }

        [HttpGet("send-invoice/{orderId}")]
        public async Task<IActionResult> SendInvoiceAsync(int orderId)
        {
            try
            {
                // Generate the invoice using the service
                var invoiceDTO = await _invoiceService.GenerateInvoiceAsync(orderId);

                // Send invoice email here (you can call EmailService)
                string subject = $"Invoice for Order #{invoiceDTO.OrderId}";
                await _emailService.SendInvoiceEmailAsync(invoiceDTO.CustomerEmail, subject, invoiceDTO); // Pass the invoiceDTO here

                // Return the invoice DTO as the response
                return Ok(invoiceDTO);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., order not found, etc.)
                return BadRequest(new { message = ex.Message });
            }
        }

        private string GenerateEmailBody(InvoiceDTO invoiceDTO)
        {
            var emailBody = $@"
                <h1>Invoice for Order #{invoiceDTO.OrderId}</h1>
                <p>Dear {invoiceDTO.CustomerName},</p>
                <p>Thank you for your order from {invoiceDTO.RestaurantName}!</p>
                <p><strong>Order Details:</strong></p>
                <ul>
                    {string.Join("", invoiceDTO.Items.Select(item =>
                        $"<li>{item.Quantity} x {item.ItemName} - {item.Price:C} each (Total: {item.Quantity * item.Price:C})</li>"))}
                </ul>
                <p><strong>Total Amount: {invoiceDTO.TotalAmount:C}</strong></p>
                <p><strong>Delivery Address:</strong> {invoiceDTO.DeliveryAddress}</p>
                <p><strong>Order Date:</strong> {invoiceDTO.OrderDate:MMMM dd, yyyy}</p>
                <p>We hope you enjoy your meal!</p>
                <p>Best regards, <br/> The {invoiceDTO.RestaurantName} Team</p>
            ";

            return emailBody;
        }


    }
}
