namespace NikeShoeStoreApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public ICollection<Order> Orders { get; set; }
        

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public bool IsActive { get; set; } = true;
    }
}
