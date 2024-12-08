using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.DataTransferObject;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

public class InvoiceService
{
    private readonly FoodDeliveryContext _context;

    public InvoiceService(FoodDeliveryContext context)
    {
        _context = context;
    }

    public async Task<InvoiceDTO> GenerateInvoiceAsync(int orderId)
    {
        // Fetch the order from the database, including related entities (OrderItems, User, Restaurant, MenuItems)
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            // Handle case when order is not found
            throw new Exception("Order not found.");
        }

        // Map to InvoiceDTO
        var invoiceDTO = new InvoiceDTO
        {
            OrderId = order.OrderId,
            CustomerName = order.User?.Username,
            CustomerEmail = order.User?.Email,
            DeliveryAddress = order.DeliveryAddress,
            RestaurantName = order.Restaurant?.Name,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems?.Select(o => new InvoiceItemDTO
            {
                ItemName = o.MenuItem?.Name,
                Description = o.MenuItem?.Description,
                Quantity = o.Quantity,
                Price = o.Price
            }).ToList()
        };

        return invoiceDTO;
    }
    
}
