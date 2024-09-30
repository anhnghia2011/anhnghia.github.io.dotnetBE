namespace NikeShoeStoreApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } // Khách hàng đặt hàng
        public DateTime OrderDate { get; set; } // Ngày đặt hàng
        public decimal TotalAmount { get; set; } // Tổng tiền
        public Customer Customer { get; set; } // Tham chiếu đến khách hàng
        public ICollection<OrderDetail> OrderDetails { get; set; } // Chi tiết đơn hàng
    }
}
