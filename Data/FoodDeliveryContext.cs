using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace FoodDeliveryManagementSystem.Data
{
    public class FoodDeliveryContext : DbContext
    {
        // DbSets for all the entities
        public DbSet<User> Users { get; set; }
      
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<OrderTracking> OrderTrackings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
     //   public DbSet<PaymentDetails> PaymentDetails { get; set; }   

        // Constructor
        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options)
            : base(options)
        { }

        // Configure relationships and other model configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            // Configure User and Role-based relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete for orders

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete for deliveries

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Order)
                .WithMany()
                .HasForeignKey(f => f.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // If order is deleted, feedback is also deleted

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete for feedbacks

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete for reports

            // Configure Order and Restaurant relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Restaurant)
                .WithMany()
                .HasForeignKey(o => o.RestaurantId);

            // Configure Delivery and Order relationships
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithMany()
                .HasForeignKey(d => d.OrderId);

            modelBuilder.Entity<MenuItem>()
                .HasKey(mi => mi.ItemId);

            // Configure Restaurant and MenuItem relationships
            modelBuilder.Entity<MenuItem>()
             .HasOne(m => m.Restaurant)  // MenuItem has one Restaurant
             .WithMany(r => r.MenuItems) // A Restaurant can have many MenuItems
             .HasForeignKey(m => m.RestaurantId)  // RestaurantId is the foreign key
             .OnDelete(DeleteBehavior.SetNull);
            // Configure Promotions and Restaurant relationships
            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Restaurant)
                .WithMany()
                .HasForeignKey(p => p.RestaurantId);

            modelBuilder.Entity<Inventory>()
        .HasKey(i => i.InventoryId);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.MenuItem) // Link Inventory to MenuItem
                .WithMany()
                .HasForeignKey(i => i.ItemId) // Add ItemId foreign key
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Restaurant) // Link Inventory to Restaurant
                .WithMany()
                .HasForeignKey(i => i.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderTracking>()
                .HasKey(ot => ot.TrackingId);
            // Configure OrderTracking and Order relationships
            modelBuilder.Entity<OrderTracking>()
                .HasOne(ot => ot.Order)
                .WithMany()
                .HasForeignKey(ot => ot.OrderId);

            modelBuilder.Entity<Restaurant>()
    .HasOne(r => r.Owner)  // A Restaurant has one Owner
    .WithMany()  // A User can own many Restaurants
    .HasForeignKey(r => r.OwnerId)  // OwnerId is the foreign key in Restaurant
    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Cart>()
                .HasKey(c => c.CartId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Restaurant)
                .WithMany()
                .HasForeignKey(c => c.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasKey(ci => ci.CartItemId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.MenuItem)
                .WithMany()
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.OrderItemId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
    .HasMany(o => o.OrderItems) // Define the relationship
    .WithOne(oi => oi.Order) // Reference back to Order in OrderItem
    .HasForeignKey(oi => oi.OrderId) // Specify the foreign key
    .OnDelete(DeleteBehavior.Cascade); // Set cascading delete
           

        }
    }

}
