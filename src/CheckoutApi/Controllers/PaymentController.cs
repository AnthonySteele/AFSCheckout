using System;
using System.Net;
using System.Threading.Tasks;
using CheckoutApi.Bank;
using CheckoutApi.Repository;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CheckoutApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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

            var response = await _paymentService.ProcessPayment(request);

            return Ok(response);
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
                var paymentData = _paymentService.GetPaymentById(guidId);

                if (paymentData != null)
                {
                    return Ok(paymentData);
                }
            }

            return NotFound();
        }
    }
}
