using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class DBContextHangHoa : DbContext
    {
        public DBContextHangHoa(DbContextOptions<DBContextHangHoa> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
