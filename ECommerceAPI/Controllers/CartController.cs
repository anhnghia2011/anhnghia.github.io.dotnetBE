using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models;
using NikeShoeStoreApi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost("add")]
        public async Task<ActionResult<CartItems>> AddToCart([FromBody] CartItems cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest("Invalid cart item.");
            }
            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            cartItem.ProductName = product.Name;
            cartItem.ProductImage = product.ImageUrl;
            cartItem.Price = product.Price;
            var existingCartItem = _context.CartItems
                .FirstOrDefault(c => c.ProductId == cartItem.ProductId && c.Size == cartItem.Size);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
                existingCartItem.ProductName = product.Name;
                existingCartItem.ProductImage = product.ImageUrl;

                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                _context.CartItems.Add(cartItem);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCart), new { id = cartItem.Id }, cartItem);
        }

        [HttpGet]
        public async Task<ActionResult<List<CartItems>>> GetCart()
        {
            var cartItems = await _context.CartItems.ToListAsync(); 
            return Ok(cartItems);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<CartItems>> UpdateCartItem(int id, [FromBody] UpdateQuantityRequest request)
        {
            if (request.Quantity < 1)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }
            cartItem.Quantity = request.Quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return Ok(cartItem);
        }
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCartItems()
        {
            var cartItems = await _context.CartItems.ToListAsync();

            if (cartItems.Count == 0)
            {
                return NoContent();
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
