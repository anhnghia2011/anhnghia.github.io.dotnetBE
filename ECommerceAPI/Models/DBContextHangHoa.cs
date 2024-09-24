using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class DBContextHangHoa : DbContext
    {
        public DBContextHangHoa(DbContextOptions<DBContextHangHoa> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
