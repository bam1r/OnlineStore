using System;
using System.Threading.Tasks;
using PaymentsService.Models;

namespace PaymentsService.Services
{
    /// <summary>
    /// Интерфейс сервиса для управления платёжными аккаунтами и платежами.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>Создать платёжный аккаунт для пользователя.</summary>
        Task<PaymentAccount> CreateAccountAsync(int userId);
        /// <summary>Получить платёжный аккаунт пользователя.</summary>
        Task<PaymentAccount> GetAccountAsync(int userId);
        /// <summary>Пополнить баланс платёжного аккаунта пользователя.</summary>
        Task<(bool Success, string Message, decimal NewBalance)> TopUpAccountAsync(int userId, decimal amount);
        /// <summary>Обработать платёж по заказу.</summary>
        Task<(bool Success, string Message, decimal NewBalance)> ProcessPaymentAsync(int orderId, int userId, decimal amount);
    }
} 