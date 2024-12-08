namespace FoodDeliveryManagementSystem.DataTransferObject
{
    public class InvoiceItemDTO
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
