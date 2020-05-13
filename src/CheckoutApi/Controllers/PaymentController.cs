using System;
using System.Net;
using System.Threading.Tasks;
using CheckoutApi.Bank;
using CheckoutApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace CheckoutApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController: ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IBankService bankService,
            IPaymentRepository paymentRepository,
            ILogger<PaymentController> logger)
        {
            _bankService = bankService;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        [HttpPut]
        [SwaggerResponse(HttpStatusCode.OK, typeof(BankResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, null)]
        public async Task<IActionResult> Put([FromBody]PaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            _logger.LogInformation($"Payment request for {request.Amount} {request.Currency} recieved");

            var response = await ProcessPayment(request);

            return Ok(response);
        }

        private async Task<BankResponse> ProcessPayment(PaymentRequest request)
        {
            var response = await _bankService.ProcessPayment(request);

            var data = new PaymentData
            {
                TransactionId = response.TransactionId,
                Status = response.Success ? PaymentStatus.Accepted : PaymentStatus.Rejected,
                CardNumber = request.CardNumber,
                Amount = request.Amount,
                Created = DateTimeOffset.UtcNow
            };
            _paymentRepository.Save(data);
            return response;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaymentData))]
        [SwaggerResponse(HttpStatusCode.NotFound, null)]
        [SwaggerResponse(HttpStatusCode.BadRequest, null)]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            if (Guid.TryParse(id, out var guidId))
            {
                var data = _paymentRepository.Read(guidId);

                if (data != null)
                {
                    return Ok(data);
                }
            }

            return NotFound();
        }
    }
}
