using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeShoeStoreApi.Data;
using NikeShoeStoreApi.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;

namespace NikeShoeStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CustomersController(DBContextNikeShoeStore context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Customer>> Register(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _context.Customers.AnyAsync(c => c.Email == registerDto.Email))
            {
                return BadRequest("Email already in use.");
            }

            // Create new customer
            var customer = new Customer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Password = HashPassword(registerDto.Password) // Mã hóa mật khẩu
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<Customer>> Login(LoginDto loginDto)
        {
            // Find user by email
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Email == loginDto.Email);
            if (customer == null || !VerifyPassword(loginDto.Password, customer.Password)) // So sánh mật khẩu đã mã hóa
            {
                return BadRequest("Invalid credentials.");
            }

            return Ok(new
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            });
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            // Tìm người dùng theo ID
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("User not found.");
            }

            // Xóa tài khoản
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            // Trả về thông báo thành công
            return NoContent(); // Trả về 204 No Content khi xóa thành công
        }

        // Phương thức để mã hóa mật khẩu
        private string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key; // Tạo salt từ key của HMAC
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Mã hóa mật khẩu
                // Lưu trữ salt và hash ở nơi thích hợp
                return Convert.ToBase64String(passwordHash);
            }
        }

        // Phương thức để xác thực mật khẩu
        private bool VerifyPassword(string password, string storedHash)
        {
            var storedHashBytes = Convert.FromBase64String(storedHash);
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHashBytes);
            }
        }
    }
}
