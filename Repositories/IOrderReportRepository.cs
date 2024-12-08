using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IOrderReportRepository
    {
        Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<IGrouping<string, Order>>> GetOrdersGroupedByDateAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null, string groupBy = "day");
        Task<IEnumerable<IGrouping<string, OrderItem>>> GetOrdersGroupedByCategoryAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
