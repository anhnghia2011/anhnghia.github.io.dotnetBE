namespace NikeShoeStoreApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } // Tên
        public string LastName { get; set; } // Họ
        public string Email { get; set; } // Địa chỉ email
        public string PhoneNumber { get; set; } // Số điện thoại
        public ICollection<Order> Orders { get; set; } // Danh sách đơn hàng của khách hàng
    }
}
