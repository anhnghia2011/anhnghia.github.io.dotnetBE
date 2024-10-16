using NikeShoeStoreApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeShoeStoreApi.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;

        public OrderDetailsController(DBContextNikeShoeStore context)
        {
            _context = context;
        }

        // GET: api/orderdetails
        [HttpGet]
        public ActionResult<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            return _context.OrderDetails.ToList();
        }

        // GET: api/orderdetails/{id}
        [HttpGet("{id}")]
        public ActionResult<OrderDetail> GetOrderDetail(int id)
        {
            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return orderDetail;
        }

        // POST: api/orderdetails
        [HttpPost]
        public ActionResult<OrderDetail> CreateOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.OrderDetailId }, orderDetail);
        }

        // PUT: api/orderdetails/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderDetails.Any(e => e.OrderDetailId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/orderdetails/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrderDetail(int id)
        {
            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();

            return NoContent();
        }
    }
}