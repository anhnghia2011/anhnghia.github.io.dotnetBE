namespace NikeShoeStoreApi.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; } 
        public string ImageUrl { get; set; } 
        public int CategoryId { get; set; } 
        public string Gender { get; set; } 
        public bool IsNew { get; set; } 
        public decimal? DiscountPrice { get; set; } 
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
        public bool Spotlight { get; set; }
        public Category Category { get; set; } 
        public decimal GetFinalPrice()
        {
            return DiscountPrice.HasValue && DiscountPrice.Value < Price ? DiscountPrice.Value : Price;
        }
    }
}
