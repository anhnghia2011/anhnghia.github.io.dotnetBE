using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models;
using System.Collections.Generic;
using System.Linq;
using NikeShoeStoreApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;

        public OrdersController(DBContextNikeShoeStore context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return _context.Orders.ToList();
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder(Order order)
        {
            // Kiểm tra nếu order là null
            if (order == null)
            {
                return BadRequest("Order cannot be null.");
            }

            // Kiểm tra trường 'order' trong OrderDetails
            if (order.OrderDetails == null || !order.OrderDetails.Any())
            {
                return BadRequest("Order must have at least one order detail.");
            }

            foreach (var detail in order.OrderDetails)
            {
                if (string.IsNullOrWhiteSpace(detail.Order))
                {
                    return BadRequest("Each order detail must have a valid order field.");
                }

                if (detail.ProductId <= 0)
                {
                    return BadRequest("Each order detail must have a valid product ID.");
                }

                if (detail.Quantity <= 0)
                {
                    return BadRequest("Each order detail must have a valid quantity.");
                }

                if (detail.UnitPrice <= 0)
                {
                    return BadRequest("Each order detail must have a valid unit price greater than 0.");
                }

                // Tính tổng giá nếu cần thiết
                detail.TotalPrice = detail.Quantity * detail.UnitPrice;
            }

            // Lưu đơn hàng vào cơ sở dữ liệu
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "An error occurred while saving the order.", details = dbEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An unexpected error occurred.", details = ex.Message });
            }

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }


    }
}