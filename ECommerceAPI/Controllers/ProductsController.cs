using NikeShoeStoreApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeShoeStoreApi.Models;

namespace NikeShoeStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;

        public ProductsController(DBContextNikeShoeStore context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return product;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }

            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("Category does not exist.");
            }

            product.Category = category;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("Category does not exist.");
            }

            product.Category = category;
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound("Product not found.");
                }
                else
                {
                    throw; 
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpGet("gender/{gender}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByGender(string gender)
        {
            var products = await _context.Products
                .Where(p => p.Gender.ToLower() == gender.ToLower())
                .Include(p => p.Category)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found for the specified gender.");
            }

            return products;
        }


        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found for the specified category.");
            }

            return products;
        }

        [HttpGet("spotlight")]
        public async Task<ActionResult<IEnumerable<Product>>> GetSpotlightProducts()
        {
            var products = await _context.Products
                .Where(p => p.Spotlight)
                .Include(p => p.Category)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No spotlight products found.");
            }

            return products;
        }
    }
}
