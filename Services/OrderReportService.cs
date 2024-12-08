using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class OrderReportService:IOrderReportService
    {
        private readonly IOrderReportRepository _orderReportRepository;

        public OrderReportService(IOrderReportRepository orderReportRepository)
        {
            _orderReportRepository = orderReportRepository;
        }

        // Get orders grouped by category
        public async Task<IEnumerable<dynamic>> GetOrdersGroupedByCategoryAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _orderReportRepository.GetOrdersGroupedByCategoryAsync(restaurantId, startDate, endDate);
        }

        // Get orders by restaurant ID
        public async Task<IEnumerable<dynamic>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _orderReportRepository.GetOrdersByRestaurantIdAsync(restaurantId, startDate, endDate);
        }

        // Get orders grouped by date
        public async Task<IEnumerable<dynamic>> GetOrdersGroupedByDateAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null, string frequency = "daily")
        {
            return await _orderReportRepository.GetOrdersGroupedByDateAsync(restaurantId, startDate, endDate, frequency);
        }
    }
}
