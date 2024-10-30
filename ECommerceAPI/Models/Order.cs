using System;
using System.Collections.Generic;

namespace NikeShoeStoreApi.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public List<CartItems> CartItems { get; set; }
    }
}
