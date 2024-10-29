using NikeShoeStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NikeShoeStoreApi.Services;

namespace NikeShoeStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly EmailService _emailService;

        public FeedbackController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null || string.IsNullOrWhiteSpace(feedback.Message))
            {
                return BadRequest("Invalid feedback data.");
            }

            await _emailService.SendFeedbackEmail(feedback);
            return Ok("Feedback sent successfully!");
        }
    }
}
