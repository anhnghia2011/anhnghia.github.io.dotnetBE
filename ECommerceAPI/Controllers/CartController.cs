using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models; // Make sure you have a CartItem model
using NikeShoeStoreApi.Data;


namespace NikeShoeStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;

        public CartController(DBContextNikeShoeStore context)
        {
            _context = context;
        }

        // Add item to the cart
        [HttpPost("add")]
        public ActionResult<CartItems> AddToCart([FromBody] CartItems cartItem)
        {
            // Validate the cartItem here (e.g., check if product exists, etc.)
            if (cartItem == null)
            {
                return BadRequest("Invalid cart item.");
            }

            // Add the item to the cart (this could involve more complex logic)
            _context.CartItems.Add(cartItem); // Ensure you have a CartItems DbSet
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCart), new { id = cartItem.Id }, cartItem); // Assuming you have a GetCart method
        }

        // Placeholder method for getting the cart items
        [HttpGet]
        public ActionResult<List<CartItems>> GetCart()
        {
            // Retrieve cart items for the current user/session
            var cartItems = _context.CartItems.ToList(); // Implement user-specific logic if necessary
            return Ok(cartItems);
        }
    }
}
