using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models; // Ensure CartItem and Product models exist
using NikeShoeStoreApi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

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
        public async Task<ActionResult<CartItems>> AddToCart([FromBody] CartItems cartItem)
        {
            // Validate the cartItem input
            if (cartItem == null)
            {
                return BadRequest("Invalid cart item.");
            }

            // Check if the product exists in the catalog
            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Populate additional product details (name, image) in the cart item
            cartItem.ProductName = product.Name;
            cartItem.ProductImage = product.ImageUrl;
            cartItem.Price = product.Price; // Assuming product model has Price property

            // Check if the same product with the same size already exists in the cart
            var existingCartItem = _context.CartItems
                .FirstOrDefault(c => c.ProductId == cartItem.ProductId && c.Size == cartItem.Size);

            if (existingCartItem != null)
            {
                // If the product exists in the cart, just increase the quantity
                existingCartItem.Quantity += cartItem.Quantity;
                // Optional: If needed, you can update the ProductName and ProductImage again
                existingCartItem.ProductName = product.Name;
                existingCartItem.ProductImage = product.ImageUrl;

                _context.CartItems.Update(existingCartItem); // Update the existing item
            }
            else
            {
                // Add the new item to the cart
                _context.CartItems.Add(cartItem);
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return the added or updated cart item
            return CreatedAtAction(nameof(GetCart), new { id = cartItem.Id }, cartItem);
        }

        // Get all items in the cart (for simplicity, assuming a single user or session-based cart)
        [HttpGet]
        public async Task<ActionResult<List<CartItems>>> GetCart()
        {
            // Retrieve cart items (implement user-specific logic if necessary)
            var cartItems = await _context.CartItems.ToListAsync(); // Consider user/session filtering
            return Ok(cartItems);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<CartItems>> UpdateCartItem(int id, [FromBody] UpdateQuantityRequest request)
        {
            // Validate the new quantity
            if (request.Quantity < 1)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            // Find the cart item by Id
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Update the quantity
            cartItem.Quantity = request.Quantity;

            // Save the changes to the database
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return Ok(cartItem);
        }

        // Optionally, implement a method to remove an item from the cart
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> RemoveFromCart(int id)
        {
            // Find the cart item by Id
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Remove the item from the cart
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCartItems()
        {
            // Lấy tất cả cart items từ cơ sở dữ liệu
            var cartItems = await _context.CartItems.ToListAsync();

            if (cartItems.Count == 0)
            {
                return NoContent(); // Nếu không có sản phẩm nào trong giỏ, trả về 204 No Content
            }

            _context.CartItems.RemoveRange(cartItems); // Xóa tất cả các mục trong giỏ hàng
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

            return NoContent(); 
        }
    }
}
