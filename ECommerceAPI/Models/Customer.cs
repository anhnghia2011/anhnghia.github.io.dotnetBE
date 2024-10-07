namespace NikeShoeStoreApi.Models
{
    public class Customer
    {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; } // Lưu trữ mật khẩu dưới dạng plain text
            public ICollection<Order> Orders { get; set; }
        

            // Có thể thêm thuộc tính trạng thái, ví dụ: 
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo
            public bool IsActive { get; set; } = true; // Trạng thái tài khoản
    }
}
