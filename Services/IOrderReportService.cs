namespace FoodDeliveryManagementSystem.Services
{
    public interface IOrderReportService
    {
       public  Task<IEnumerable<dynamic>> GetOrdersGroupedByCategoryAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null);
       public Task<IEnumerable<dynamic>> GetOrdersByRestaurantIdAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null);
      public  Task<IEnumerable<dynamic>> GetOrdersGroupedByDateAsync(int restaurantId, DateTime? startDate = null, DateTime? endDate = null, string frequency = "daily");
    }
}
