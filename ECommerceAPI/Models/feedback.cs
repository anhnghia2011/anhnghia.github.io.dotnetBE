using System.ComponentModel.DataAnnotations;

namespace NikeShoeStoreApi.Models // Sử dụng namespace đúng
{
    public class Feedback
    {
        [Key] // Đánh dấu thuộc tính này là khóa chính
        public int Id { get; set; } // Khóa chính

        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
