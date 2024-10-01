using ECommerceAPI.Data;
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

        // Lấy danh sách tất cả sản phẩm
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        // Lấy thông tin sản phẩm theo id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // Tạo sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }

            // Kiểm tra xem danh mục có tồn tại không bằng cách sử dụng CategoryId
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("Category does not exist.");
            }

            // Gán danh mục cho sản phẩm
            product.Category = category;

            // Thêm sản phẩm vào cơ sở dữ liệu
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // Cập nhật sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            // Kiểm tra xem danh mục có tồn tại không trước khi cập nhật
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("Category does not exist.");
            }

            // Gán danh mục cho sản phẩm
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
                    throw; // Ném lại ngoại lệ để xử lý sau
                }
            }

            return NoContent();
        }

        // Xóa sản phẩm
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

        // Kiểm tra xem sản phẩm có tồn tại hay không
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        // Lấy danh sách sản phẩm theo giới tính
        [HttpGet("gender/{gender}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByGender(string gender)
        {
            var products = await _context.Products
                .Where(p => p.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase))
                .Include(p => p.Category)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found for the specified gender.");
            }

            return products;
        }
    }
}
