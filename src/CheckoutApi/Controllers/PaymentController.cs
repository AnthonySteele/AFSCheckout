using System.Threading.Tasks;
using CheckoutApi.Bank;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CheckoutApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController: ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IBankService bankService, ILogger<PaymentController> logger)
        {
            _bankService = bankService;
            _logger = logger;
        }

        public async Task<IActionResult> Put([FromBody]PaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            _logger.LogInformation($"Payment request for {request.Amount} {request.Currency} recieved");

            await _bankService.ProcessPayment(request);

            return Ok();
        }
    }
}
