using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeShoeStoreApi.Data;
using NikeShoeStoreApi.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace NikeShoeStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly DBContextNikeShoeStore _context;

        public CustomersController(DBContextNikeShoeStore context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Customer>> Register(RegisterDto registerDto)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == registerDto.Email))
            {
                return BadRequest("Email already in use.");
            }

            var (hashedPassword, salt) = HashPassword(registerDto.Password);

            var customer = new Customer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Password = hashedPassword, 
                Salt = salt 
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
           
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Email == loginDto.Email);
            if (customer == null || !VerifyPassword(loginDto.Password, customer.Password, customer.Salt))
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
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("User not found.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private (string hashedPassword, string salt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key); 
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); 
                return (Convert.ToBase64String(passwordHash), salt);
            }
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt); 
            using (var hmac = new HMACSHA512(saltBytes)) 
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); 
                var storedHashBytes = Convert.FromBase64String(storedHash);
                return computedHash.SequenceEqual(storedHashBytes); 
            }
        }
    }
}
