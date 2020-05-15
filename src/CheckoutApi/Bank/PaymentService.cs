using System;
using System.Threading.Tasks;
using CheckoutApi.Controllers;
using CheckoutApi.Repository;
using Microsoft.Extensions.Logging;

namespace CheckoutApi.Bank
{
    public class PaymentService : IPaymentService
    {
        private readonly IAquiringBankService _bankService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IAquiringBankService bankService,
            IPaymentRepository paymentRepository,
            ILogger<PaymentService> logger)
        {
            _bankService = bankService;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<PaymentData> ProcessPayment(PaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var paymentData = BuildPaymentData(request);
            _paymentRepository.Add(paymentData);

            CustomMetrics.PaymentStarted.Inc();
            _logger.LogInformation($"Payment request for {request.Amount} {request.Currency} recieved and given id {paymentData.Id}");

            var bankResponse = await _bankService.ProcessPayment(request);

            var newStatus = bankResponse.Success ? PaymentStatus.Accepted : PaymentStatus.Rejected;
            _paymentRepository.Update(paymentData.Id, bankResponse.TransactionId, newStatus);

            var statusString = bankResponse.Success ? "success" : "fail";
            _logger.LogInformation($"Transaction {bankResponse.TransactionId} {statusString}");
            CustomMetrics.PaymentCompleted.WithLabels(statusString).Inc();

            return paymentData;
        }

        private static PaymentData BuildPaymentData(PaymentRequest request)
        {
            return new PaymentData
            {
                Id = Guid.NewGuid(),
                BankTransactionId = null,
                Status = PaymentStatus.Received,
                CardNumber = request.CardNumber.Substring(0, 4),
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
