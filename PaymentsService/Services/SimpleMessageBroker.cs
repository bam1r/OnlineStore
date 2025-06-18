using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PaymentsService.Services
{
    /// <summary>
    /// Простой брокер сообщений для имитации асинхронной обработки платежей.
    /// </summary>
    public class SimpleMessageBroker : IMessageBroker
    {
        private readonly ILogger<SimpleMessageBroker> _logger;

        public SimpleMessageBroker(ILogger<SimpleMessageBroker> logger)
        {
            _logger = logger;
        }
        
        public async Task ProcessPaymentAsync(int orderId, int userId, decimal amount)
        {
            _logger.LogInformation($"Starting payment processing for order {orderId}");
            
            await Task.Delay(2000);
            
            _logger.LogInformation($"Payment processing completed for order {orderId}");
        }
    }
} 