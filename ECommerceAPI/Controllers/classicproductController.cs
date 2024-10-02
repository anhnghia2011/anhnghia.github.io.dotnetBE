using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class classicproductController : ControllerBase
    {
        private static List<ProductClassic> _products = new List<ProductClassic>(); // Danh sách sản phẩm

        // Lấy tất cả sản phẩm
        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            return Ok(_products);
        }

        // Thêm sản phẩm mới
        [HttpPost]
        public ActionResult<ProductClassic> CreateProduct([FromBody] ProductClassic productClassic)
        {
            // Tự động gán ID cho sản phẩm mới
            productClassic.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(productClassic);
            return CreatedAtAction(nameof(GetProducts), new { id = productClassic.Id }, productClassic);
        }

        // Xóa sản phẩm theo ID
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _products.Remove(product);
            return NoContent();
        }
    }
}

