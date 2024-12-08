using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System;


namespace FoodDeliveryManagementSystem.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly EmailService _emailService;
        private readonly FoodDeliveryContext _context;
       

        public InventoryService(IInventoryRepository inventoryRepository, FoodDeliveryContext context,EmailService emailService)
        {
            _inventoryRepository = inventoryRepository;
            _context = context;
            _emailService = emailService;
          
        }
        public async Task<List<Inventory>> GetAllInventories()
        {
            // Ensure that the method is asynchronous and return a Task
            return (await _inventoryRepository.GetAll()).ToList();
        }


        public List<Inventory> GetLowStockItemsBasedOnOrders(int restaurantId)
        {
            return _inventoryRepository.GetLowStockItemsBasedOnOrders(restaurantId);
        }

        public async Task TrackInventoryAsync()
        {
            var today = DateTime.UtcNow.Date;

            // Get orders for today
            var restaurantOrders = _context.OrderItems
                .Join(
                    _context.Orders,
                    oi => oi.OrderId,
                    o => o.OrderId,
                    (oi, o) => new { OrderItem = oi, Order = o }
                )
                .Where(joined => joined.Order.OrderDate.Date == today)
                .GroupBy(joined => new { joined.Order.RestaurantId, joined.OrderItem.ItemId })
                .Select(g => new
                {
                    RestaurantId = g.Key.RestaurantId,
                    ItemId = g.Key.ItemId,
                    OrderCount = g.Count() // Count of orders per item per restaurant
                })
                .Where(r => r.OrderCount > 5) // Threshold for low stock
                .ToList();

            foreach (var groupedOrder in restaurantOrders)
            {
                // Get inventory for the current restaurant and item
                var inventory = await _inventoryRepository.GetByItemIdAndRestaurantIdAsync(groupedOrder.ItemId, groupedOrder.RestaurantId);
                if (inventory != null)
                {
                    // Deduct usage based on a placeholder logic
                    inventory.Quantity -= 1;//CalculateItemUsage(groupedOrder.RestaurantId, groupedOrder.ItemId);

                    // Check if inventory is below reorder level
                    if (inventory.Quantity <= inventory.ReorderLevel)
                    {
                        inventory.Status = "Low Stock";
                        await NotifyRestaurantOwnerAsync(groupedOrder.RestaurantId, inventory);
                    }

                    await _inventoryRepository.UpdateAsync(inventory);
                }
            }

            await _inventoryRepository.SaveChangesAsync();
        }
        private int CalculateItemUsage(int restaurantId, int itemId)
        {
            // Get the count of orders for this item in the restaurant
            var orderCount = _context.OrderItems
                .Where(oi => oi.ItemId == itemId && oi.Order.RestaurantId == restaurantId)
                .Count();

            return orderCount; // Return the total number of units ordered
        }
        public async Task<IEnumerable<Inventory>> GetInventoryByRestaurantIdAsync(int restaurantId)
        {
            return await _inventoryRepository.GetByRestaurantIdAsync(restaurantId);
        }

        private async Task NotifyRestaurantOwnerAsync(int restaurantId, Inventory inventory)
        {
            // Retrieve the restaurant by ID along with its owner
            var restaurant = await _context.Restaurants
                .Include(r => r.Owner)  // Include the owner (User) related to the restaurant
                .FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);

            if (restaurant?.Owner != null)
            {
                // Retrieve the owner's phone number and email
                string ownerEmail = restaurant.Owner.Email; // Assuming the owner has an Email field

                if (!string.IsNullOrEmpty(ownerEmail))
                {
                    // Prepare the email parameters
                    var subject = $"Low Stock Alert for {inventory.ItemId}";
                    var itemName = inventory.ItemId; // You can adjust this to show the correct item name
                    var currentStock = inventory.Quantity;
                    var reorderLevel = inventory.ReorderLevel;

                    // Send the low stock email
                    await _emailService.SendLowStockAlertAsync(
                        ownerEmail,
                        restaurant.Name,
                        itemName,
                        currentStock,
                        reorderLevel
                    );

                    Console.WriteLine($"Low stock email sent to {ownerEmail}.");
                }
                else
                {
                    Console.WriteLine("Restaurant owner does not have an email address.");
                }
            }
            else
            {
                Console.WriteLine("Restaurant or owner not found.");
            }
        }



        public async Task UpdateInventoryQuantityAsync(int restaurantId, int itemId, int newQuantity)
        {
            // Find the inventory for the specific restaurant and item
            var inventory = await _inventoryRepository.GetByItemIdAndRestaurantIdAsync(itemId, restaurantId);

            if (inventory == null)
            {
                throw new InvalidOperationException("Inventory item not found for this restaurant.");
            }

            // Update the quantity
            inventory.Quantity = newQuantity;

            // Optionally, check for low stock condition
            if (inventory.Quantity <= inventory.ReorderLevel)
            {
                inventory.Status = "Low Stock";
            }
            else
            {
                inventory.Status = "In Stock";
            }

            // Save the changes
            await _inventoryRepository.UpdateAsync(inventory);
            await _inventoryRepository.SaveChangesAsync();
        }
        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            // Check if inventory already exists for this ItemId and RestaurantId
            var existingInventory = await _inventoryRepository.GetByItemIdAndRestaurantIdAsync(inventory.ItemId, inventory.RestaurantId);
            if (existingInventory != null)
            {
                throw new InvalidOperationException("Inventory item already exists for this restaurant.");
            }

            // Add new inventory
            await _inventoryRepository.AddAsync(inventory);
            await _inventoryRepository.SaveChangesAsync();

            return inventory;
        }

        public async Task<Inventory?> GetByIdAsync(int inventoryId)
        {
            return await _inventoryRepository.GetByIdAsync(inventoryId);
        }
        public async Task DecreaseInventoryAsync(int itemId, int quantity)
        {
            var inventoryItem = await _inventoryRepository.GetByItemId(itemId);
            if (inventoryItem != null && inventoryItem.Quantity >= quantity)
            {
                inventoryItem.Quantity -= quantity;
                await _inventoryRepository.UpdateAsync(inventoryItem);
            }
            else
            {
                throw new Exception("Insufficient stock");
            }
        }


    }
}
