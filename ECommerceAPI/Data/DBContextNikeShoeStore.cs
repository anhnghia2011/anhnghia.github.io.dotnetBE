using Microsoft.EntityFrameworkCore;
using NikeShoeStoreApi.Models;

namespace NikeShoeStoreApi.Data
{
    public class DBContextNikeShoeStore : DbContext
    {
        public DBContextNikeShoeStore(DbContextOptions<DBContextNikeShoeStore> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<CartItems> CartItems { get; set; }

    }
}
