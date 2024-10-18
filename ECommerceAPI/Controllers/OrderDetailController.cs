using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models;
using NikeShoeStoreApi.Data;
using System.Linq;

namespace NikeShoeStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;

        public OrderDetailController(DBContextNikeShoeStore context)
        {
            _context = context;
        }

        // Lấy chi tiết đơn hàng theo ID đơn hàng
        [HttpGet("order/{orderId}")]
        public ActionResult<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
            return Ok(orderDetails);
        }

        // Thêm chi tiết vào đơn hàng
        [HttpPost]
        public ActionResult<OrderDetail> AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetOrderDetails), new { orderId = orderDetail.OrderId }, orderDetail);
        }
    }
}
