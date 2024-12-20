﻿using Microsoft.AspNetCore.Mvc;
using NikeShoeStoreApi.Models;
using NikeShoeStoreApi.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public ActionResult<List<Order>> GetOrders()
        {
            var orders = _context.Orders
                                 .Include(o => o.CartItems)
                                 .ToList();
            return Ok(orders);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(int userId)
        {
            var orders = await _context.Orders
                                       .Where(o => o.CustomerId == userId)
                                       .Include(o => o.CartItems)
                                       .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found for this user.");
            }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = _context.Orders
                                .Include(o => o.CartItems)
                                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == order.CustomerId);
            if (!customerExists)
            {
                return BadRequest("Invalid CustomerId.");
            }

            if (order.CartItems == null || !order.CartItems.Any())
            {
                return BadRequest("The order must contain at least one item.");
            }

            order.TotalAmount = order.CartItems.Sum(item => item.Price * item.Quantity);
            foreach (var item in order.CartItems)
            {
                item.Id = 0; 
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }
    }
}
