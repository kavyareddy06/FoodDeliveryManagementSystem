using FoodDeliveryManagementSystem.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class OrderStatusUpdateService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OrderStatusUpdateService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                var orders = await orderService.GetPendingOrdersAsync();

                foreach (var order in orders)
                {
                    var elapsedTime = DateTime.Now - order.OrderDate;
                    string newStatus = null;

                    if (elapsedTime.TotalMinutes >= 2 && order.Status.ToLower() == "pending")
                    {
                        newStatus = "In Preparation";
                    }
                    else if (elapsedTime.TotalMinutes >= 6 && order.Status.ToLower() == "in preparation")
                    {
                        newStatus = "Out for Delivery";
                    }
                    else if (elapsedTime.TotalMinutes >= 8 && order.Status.ToLower() == "out for delivery")
                    {
                        newStatus = "Delivered";
                    }

                    if (newStatus != null)
                    {
                        await orderService.UpdateOrderStatusAsync(order.OrderId, newStatus);
                        await emailService.SendOrderStatusEmailAsync(order.DeliveryAddress, newStatus, order);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Adjust interval as needed
        }
    }

}
