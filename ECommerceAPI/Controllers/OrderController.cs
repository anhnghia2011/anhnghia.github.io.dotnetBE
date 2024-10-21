using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models;
using NikeShoeStoreApi.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        // Lấy danh sách đơn hàng (Get all orders)
        [HttpGet]
        public ActionResult<List<Order>> GetOrders()
        {
            // Include related CartItems
            var orders = _context.Orders
                                 .Include(o => o.CartItems)
                                 .ToList();
            return Ok(orders);
        }

        // Lấy thông tin đơn hàng theo ID (Get order by ID)
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            // Find order and include related CartItems
            var order = _context.Orders
                                .Include(o => o.CartItems)
                                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        // Tạo đơn hàng mới (Create a new order)
        [HttpPost]
        public ActionResult<Order> CreateOrder(Order order)
        {
            // Check if the customer exists
            var customerExists = _context.Customers.Any(c => c.Id == order.CustomerId);
            if (!customerExists)
            {
                return BadRequest("Invalid CustomerId.");
            }

            // Validate that there are cart items in the order
            if (order.CartItems == null || !order.CartItems.Any())
            {
                return BadRequest("The order must contain at least one item.");
            }

            // Calculate total amount based on cart items
            order.TotalAmount = order.CartItems.Sum(item => item.Price * item.Quantity);

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // Cập nhật trạng thái đơn hàng (Update order status)
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
