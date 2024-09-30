namespace NikeShoeStoreApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } // Tên sản phẩm
        public string Description { get; set; } // Mô tả sản phẩm
        public decimal Price { get; set; } // Giá sản phẩm
        public string ImageUrl { get; set; } // Đường dẫn hình ảnh
        public int CategoryId { get; set; } // ID danh mục
        public Category Category { get; set; } // Tham chiếu đến danh mục

        // Thêm thuộc tính giới tính
        public string Gender { get; set; } // "Nam", "Nữ", hoặc "Trẻ em"

        // Thêm thuộc tính mới
        public bool IsNew { get; set; } // Xác định sản phẩm mới
        public decimal? DiscountPrice { get; set; } // Giá giảm (nếu có)
        public DateTime? StartDate { get; set; } // Ngày bắt đầu giảm giá
        public DateTime? EndDate { get; set; } // Ngày kết thúc giảm giá

        // Tính giá sau giảm giá
        public decimal GetFinalPrice()
        {
            return DiscountPrice.HasValue && DiscountPrice.Value < Price ? DiscountPrice.Value : Price;
        }
    }
}
