using Cozy_Haven.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cozy_Haven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentservice;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentservice=paymentService;
            
        }
        [HttpPost("Refund/{userId}/{amount}")]
        public async Task<IActionResult> Refund(int userId, float amount)
        {
            // Call the Refund method from the payment service
            var refundSuccess = await _paymentservice.Refund(userId, amount);

            if (refundSuccess)
            {
                return Ok("Refund successful");
            }
            else
            {
                return StatusCode(500, "Failed to process refund");
            }
        }

    }
}
