using Microsoft.EntityFrameworkCore;
using NikeShoeStoreApi.Models;

namespace ECommerceAPI.Data
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
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập các quy tắc cho các thực thể tại đây nếu cần

            base.OnModelCreating(modelBuilder);
        }
    }
}
