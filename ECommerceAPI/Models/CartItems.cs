namespace NikeShoeStoreApi.Models
{
    public class CartItems
    {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public int Size { get; set; }
            public string ProductName { get; set; }
            public string ProductImage { get; set; }
            public decimal Price { get; set; }
    }
}
