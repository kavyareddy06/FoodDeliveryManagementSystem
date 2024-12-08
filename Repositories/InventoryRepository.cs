using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly FoodDeliveryContext _context;

        public InventoryRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories.Include(i => i.Restaurant).ToListAsync();
        }
        public async Task<IEnumerable<Inventory>> GetAll()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Inventories
                .Where(i => i.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(int inventoryId)
        {
            return await _context.Inventories.FindAsync(inventoryId);
        }

        public async Task UpdateAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public List<Inventory> GetLowStockItemsBasedOnOrders(int restaurantId)
        {
            var lowStockItems = _context.OrderItems
                .Join(
                    _context.Orders,
                    oi => oi.OrderId,
                    o => o.OrderId,
                    (oi, o) => new { OrderItem = oi, Order = o }
                )
                .Where(joined => joined.Order.RestaurantId == restaurantId)
                .GroupBy(joined => joined.OrderItem.ItemId)
                .Select(g => new { ItemId = g.Key, OrderCount = g.Count() })
                .Where(g => g.OrderCount > 3)
                .Join(
                    _context.Inventories,
                    g => g.ItemId,
                    inv => inv.ItemId,
                    (g, inv) => inv
                )
                .ToList();

            return lowStockItems;
        }

        public async Task<Inventory?> GetByItemIdAndRestaurantIdAsync(int itemId, int restaurantId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(inv => inv.ItemId == itemId && inv.RestaurantId == restaurantId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
        }
        public async Task<Inventory> GetByItemId(int itemId)
        {
            return await _context.Inventories.FirstOrDefaultAsync(inv => inv.ItemId == itemId);
        }
    }
}
