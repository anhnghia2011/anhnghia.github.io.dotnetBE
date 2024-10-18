namespace NikeShoeStoreApi.Models
{
    public class CartItems
    {
        public int Id { get; set; } // Assuming there's a unique identifier
        public int ProductId { get; set; } // Product ID
        public int Quantity { get; set; } // Quantity of the product
        public int Size { get; set; } // Product size (if applicable)
        // Add other properties as necessary
    }
}
