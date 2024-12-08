namespace FoodDeliveryManagementSystem.DataTransferObject
{
    public class InvoiceDTO
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string DeliveryAddress { get; set; }
        public string RestaurantName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceItemDTO> Items { get; set; }
    }
}
