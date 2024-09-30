namespace NikeShoeStoreApi.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // ID đơn hàng
        public int ProductId { get; set; } // ID sản phẩm
        public int Quantity { get; set; } // Số lượng sản phẩm
        public Order Order { get; set; } // Tham chiếu đến đơn hàng
        public Product Product { get; set; } // Tham chiếu đến sản phẩm
    }
}
