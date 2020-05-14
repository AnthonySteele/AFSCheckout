using System;
using System.Threading.Tasks;
using CheckoutApi.Controllers;
using CheckoutApi.Repository;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace CheckoutApi.Bank
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankService _bankService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IBankService bankService,
            IPaymentRepository paymentRepository,
            ILogger<PaymentService> logger)
        {
            _bankService = bankService;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<BankResponse> ProcessPayment(PaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            CustomMetrics.PaymentStarted.Inc();
            _logger.LogInformation($"Payment request for {request.Amount} {request.Currency} recieved");

            var response = await _bankService.ProcessPayment(request);

            var data = BuildPaymentData(request, response);
            _paymentRepository.Save(data);

            var statusString = response.Success ? "success" : "fail";
            _logger.LogInformation($"Transaction {response.TransactionId} {statusString}");
            CustomMetrics.PaymentCompleted.WithLabels(statusString).Inc();

            return response;
        }

        private static PaymentData BuildPaymentData(PaymentRequest request, BankResponse response)
        {
            return new PaymentData
            {
                TransactionId = response.TransactionId,
                Status = response.Success ? PaymentStatus.Accepted : PaymentStatus.Rejected,
                CardNumber = request.CardNumber,
                NameOnCard = request.NameOnCard,
                Amount = request.Amount,
                Created = DateTimeOffset.UtcNow
            };
        }

        public PaymentData? GetPaymentById(Guid id)
        {
            return _paymentRepository.GetPaymentById(id);
        }
    }
}
