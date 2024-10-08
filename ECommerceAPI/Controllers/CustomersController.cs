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

        // Register a new customer
        [HttpPost("register")]
        public async Task<ActionResult<Customer>> Register(RegisterDto registerDto)
        {
            // Check if the email is already in use
            if (await _context.Customers.AnyAsync(c => c.Email == registerDto.Email))
            {
                return BadRequest("Email already in use.");
            }

            // Hash the password and generate a salt
            var (hashedPassword, salt) = HashPassword(registerDto.Password);

            // Create a new customer object
            var customer = new Customer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Password = hashedPassword, // Store the hashed password
                Salt = salt // Store the generated salt
            };

            // Save the new customer in the database
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Return the registered customer data (without the password)
            return Ok(new
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            });
        }

        // Login a customer
        [HttpPost("login")]
        public async Task<ActionResult<Customer>> Login(LoginDto loginDto)
        {
            // Find the customer by email
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Email == loginDto.Email);
            if (customer == null || !VerifyPassword(loginDto.Password, customer.Password, customer.Salt))
            {
                return BadRequest("Invalid credentials.");
            }

            // Return the logged-in customer data (without the password)
            return Ok(new
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            });
        }

        // Get all customers
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        // Delete a customer account
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

        // Hash the password and generate a salt
        private (string hashedPassword, string salt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key); // Generate a salt
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Hash the password with the salt
                return (Convert.ToBase64String(passwordHash), salt); // Return the hashed password and the salt
            }
        }

        // Verify the password during login
        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt); // Convert stored salt back to byte array
            using (var hmac = new HMACSHA512(saltBytes)) // Use the salt to initialize the HMAC
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Rehash the input password
                var storedHashBytes = Convert.FromBase64String(storedHash);
                return computedHash.SequenceEqual(storedHashBytes); // Compare the computed hash with the stored hash
            }
        }
    }
}
