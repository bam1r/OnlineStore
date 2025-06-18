using System;
using System.Threading.Tasks;

namespace PaymentsService.Services
{
    /// <summary>
    /// Интерфейс для асинхронной обработки платежей через брокер сообщений.
    /// </summary>
    public interface IMessageBroker
    {
        Task ProcessPaymentAsync(int orderId, int userId, decimal amount);
    }
} 