namespace NikeShoeStoreApi.Models
{
    public class Order
    {
        public int OrderId { get; set; } // Unique identifier for the order
        public int CustomerId { get; set; } // ID of the customer placing the order
        public DateTime OrderDate { get; set; } = DateTime.Now; // The date the order is placed, defaults to current date
        public string Status { get; set; } = "Pending"; // Default order status
        public decimal TotalAmount { get; set; } // Total amount of the order
        public List<CartItems> CartItems { get; set; } = new List<CartItems>(); // List of cart items
    }
}
