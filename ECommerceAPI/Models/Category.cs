namespace NikeShoeStoreApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } // Tên danh mục
        public string Description { get; set; } // Mô tả danh mục
        public ICollection<Product> Products { get; set; } // Sản phẩm thuộc danh mục
    }
}
