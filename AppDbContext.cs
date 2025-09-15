using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using آخرین_الماس.Models;


namespace آخرین_الماس.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data برای تست
            modelBuilder.Entity<Food>().HasData(
                new Food { Id = 1, Name = "کباب برگ", Price = 75000, ImagePath = "~/images/food1.jpg" },
                new Food { Id = 2, Name = "کباب بناب", Price = 80000, ImagePath = "~/images/food2.jpg" },
                new Food { Id = 3, Name = "جوجه کباب", Price = 60000, ImagePath = "~/images/food3.jpg" }
            );
        }
    }
}


