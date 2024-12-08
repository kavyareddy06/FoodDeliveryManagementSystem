using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class OrderReportRepository:IOrderReportRepository
    {
        private readonly FoodDeliveryContext _context;

        public OrderReportRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Orders.AsQueryable().Where(o => o.RestaurantId == restaurantId);

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<IGrouping<string, Order>>> GetOrdersGroupedByDateAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null, string groupBy = "day")
        {
            var query = _context.Orders.AsQueryable().Where(o => o.RestaurantId == restaurantId);

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            IQueryable<IGrouping<string, Order>> groupedOrders;

            if (groupBy == "week")
            {
                groupedOrders = query.GroupBy(o => $"{o.OrderDate.Year}-{o.OrderDate.Month:D2}"); // Group by Year-Month

            }
            else if (groupBy == "month")
            {
                groupedOrders = query.GroupBy(o => o.OrderDate.Month.ToString());
            }
            else
            {
                groupedOrders = query.GroupBy(o => o.OrderDate.Date.ToString()); // Default group by day
            }

            return await groupedOrders.ToListAsync();
        }

        public async Task<IEnumerable<IGrouping<string, OrderItem>>> GetOrdersGroupedByCategoryAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.OrderItems.AsQueryable()
                .Where(oi => oi.Order.RestaurantId == restaurantId);

            if (startDate.HasValue)
                query = query.Where(oi => oi.Order.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(oi => oi.Order.OrderDate <= endDate.Value);

            // Group by Category (without projecting to anonymous type)
            var groupedOrdersByCategory = query
                .GroupBy(oi => oi.MenuItem.Category);  // Return IGrouping<string, OrderItem>

            return await groupedOrdersByCategory.ToListAsync();  // This returns IEnumerable<IGrouping<string, OrderItem>>
        }

    }
}
