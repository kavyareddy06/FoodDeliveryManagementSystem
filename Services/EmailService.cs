using Postmark;
using System.Threading.Tasks;
using FoodDeliveryManagementSystem.DataTransferObject;
using PostmarkDotNet;
using FoodDeliveryManagementSystem.Models;

public class EmailService
{
    private readonly string _postmarkApiKey = "946dea25-7966-4171-85d5-f84366ae24f9";  // Replace with your Postmark API Key

    public async Task SendInvoiceEmailAsync(string recipientEmail, string subject, InvoiceDTO invoiceDTO)
    {
        var client = new PostmarkClient(_postmarkApiKey);
        var from = "21071a05q2@vnrvjiet.in";  // Replace with your email address
        var to = "21071a05q2@vnrvjiet.in";

        // Generate the email body
        var emailBody = GenerateEmailBody(invoiceDTO);

        var message = new PostmarkMessage
        {
            From = from,
            To = to,
            Subject = subject,
            HtmlBody = emailBody,  // Send as HTML email
            TextBody = emailBody   // Optionally send plain text version of the email body
        };

        // Send the email using SendMessageAsync
        var sendResult = await client.SendMessageAsync(message);

        // Compare the Status with PostmarkStatus.Ok (enum value)
        if (sendResult.Status != PostmarkStatus.Success)
        {
            throw new Exception("Failed to send email: " + sendResult.Message);
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
    public async Task SendLowStockAlertAsync(string ownerEmail, string restaurantName, int itemName, int currentStock, int reorderLevel)
    {
        var client = new PostmarkClient(_postmarkApiKey);
        var from = "21071a05q2@vnrvjiet.in";  // Replace with your email address
        var to = "21071a05q2@vnrvjiet.in";

        // Generate the low stock email body
        var emailBody = GenerateLowStockEmailBody(restaurantName, itemName, currentStock, reorderLevel);

        var message = new PostmarkMessage
        {
            From = from,
            To = to,
            Subject = "Low Stock Alert: Immediate Action Required",
            HtmlBody = emailBody,  // Send as HTML email
            TextBody = emailBody   // Optionally send plain text version of the email body
        };

        // Send the email using SendMessageAsync
        var sendResult = await client.SendMessageAsync(message);

        // Check if the message was successfully sent
        if (sendResult.Status != PostmarkStatus.Success)
        {
            throw new Exception("Failed to send low stock alert: " + sendResult.Message);
        }
    }

    private string GenerateLowStockEmailBody(string restaurantName, int itemName, int currentStock, int reorderLevel)
    {
        var emailBody = $@"
            <h1>Low Stock Alert for {restaurantName}</h1>
            <p>Dear Restaurant Owner,</p>
            <p>We are notifying you that the item '{itemName}' is running low on stock.</p>
            <p><strong>Current Stock:</strong> {currentStock}</p>
            <p><strong>Reorder Level:</strong> {reorderLevel}</p>
            <p>Please reorder this item as soon as possible to avoid running out of stock.</p>
            <p>Best regards, <br/> The {restaurantName} Team</p>
        ";

        return emailBody;
    }
    public async Task SendOrderStatusEmailAsync(string recipientEmail, string orderStatus, Order order)
    {
        var client = new PostmarkClient(_postmarkApiKey);
        var from = "21071a05q2@vnrvjiet.in";  // Replace with your email address
        var to = "21071a05q2@vnrvjiet.in";

        // Generate the order status email body
        var emailBody = GenerateOrderStatusEmailBody(orderStatus, order);

        var message = new PostmarkMessage
        {
            From = from,
            To = to,
            Subject = $"Order #{order.OrderId} Status Update: {orderStatus}",
            HtmlBody = emailBody,  // Send as HTML email
            TextBody = emailBody   // Optionally send plain text version of the email body
        };

        // Send the email using SendMessageAsync
        var sendResult = await client.SendMessageAsync(message);

        // Check if the email was successfully sent
        if (sendResult.Status != PostmarkStatus.Success)
        {
            throw new Exception($"Failed to send order status email: {sendResult.Message}");
        }
    }

    private string GenerateOrderStatusEmailBody(string orderStatus, Order order)
    {
        var emailBody = $@"
        <h1>Order Status Update</h1>
        <p>Dear {order.UserId},</p>
        <p>From{order.RestaurantId}</p>
        <p>Your order with ID <strong>#{order.OrderId}</strong> is now <strong>{orderStatus}</strong>.</p>
        <p>Details:</p>
        <ul>
            
            <li><strong>Delivery Address:</strong> {order.DeliveryAddress}</li>
            <li><strong>Order Date:</strong> {order.OrderDate:MMMM dd, yyyy}</li>
            <li><strong>Current Status:</strong> {orderStatus}</li>
        </ul>
        <p>Thank you for choosing us!</p>
        <p>Best regards, <br/> The Dear Customer Team</p>
    ";

        return emailBody;
    }

}
