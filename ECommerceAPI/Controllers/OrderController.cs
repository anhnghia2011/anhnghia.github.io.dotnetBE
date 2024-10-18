using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models;
using NikeShoeStoreApi.Data;
using System.Linq;

namespace NikeShoeStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;

        public OrderController(DBContextNikeShoeStore context)
        {
            _context = context;
        }

        // Lấy danh sách đơn hàng
        [HttpGet]
        public ActionResult<List<Order>> GetOrders()
        {
            var orders = _context.Orders.ToList();
            return Ok(orders);
        }

        // Lấy thông tin đơn hàng theo ID
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        // Tạo đơn hàng mới
        [HttpPost]
        public ActionResult<Order> CreateOrder(Order order)
        {
            var customerExists = _context.Customers.Any(c => c.Id == order.CustomerId);
            if (!customerExists)
            {
                return BadRequest("Invalid CustomerId.");
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }
        // Cập nhật trạng thái đơn hàng
        [HttpPut("{id}")]
        public ActionResult UpdateOrderStatus(int id, [FromBody] string status)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = status;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
