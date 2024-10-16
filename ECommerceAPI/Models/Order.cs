namespace NikeShoeStoreApi.Models
{
    public class Order
    {
            public int OrderId { get; set; }
            public int CustomerId { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public decimal TotalAmount { get; set; }
            public List<OrderDetail> OrderDetails { get; set; }
        }
}
