using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CheckoutApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController: ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Put([FromBody]PaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            _logger.LogInformation($"Payment request for {request.Amount} {request.Currency} recieved");

            return Ok();
        }
    }
}
