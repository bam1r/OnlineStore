using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using PaymentsService.Models;

namespace PaymentsService.Services
{
    /// <summary>
    /// Сервис для управления платёжными аккаунтами и обработкой платежей.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        /// <summary>Хранилище аккаунтов пользователей.</summary>
        private static readonly ConcurrentDictionary<int, PaymentAccount> _accounts = new();
        /// <summary>Следующий идентификатор аккаунта.</summary>
        private static int _nextId = 1;
        /// <summary>Брокер сообщений для асинхронной обработки платежей.</summary>
        private readonly IMessageBroker _messageBroker;

        public PaymentService(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        /// <summary>
        /// Создать платёжный аккаунт для пользователя.
        /// </summary>
        public async Task<PaymentAccount> CreateAccountAsync(int userId)
        {
            if (_accounts.ContainsKey(userId))
            {
                throw new InvalidOperationException("Account already exists for this user");
            }

            var account = new PaymentAccount
            {
                Id = _nextId++,
                UserId = userId,
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };

            _accounts.TryAdd(userId, account);

            return account;
        }

        /// <summary>
        /// Получить платёжный аккаунт пользователя.
        /// </summary>
        public async Task<PaymentAccount> GetAccountAsync(int userId)
        {
            if (!_accounts.TryGetValue(userId, out var account))
            {
                throw new InvalidOperationException("Account not found");
            }

            return account;
        }

        /// <summary>
        /// Пополнить баланс платёжного аккаунта пользователя.
        /// </summary>
        public async Task<(bool Success, string Message, decimal NewBalance)> TopUpAccountAsync(int userId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero");
            }

            if (!_accounts.TryGetValue(userId, out var account))
            {
                throw new InvalidOperationException("Account not found");
            }

            account.Balance += amount;
            account.LastModifiedAt = DateTime.UtcNow;

            return (true, "Account topped up successfully", account.Balance);
        }

        /// <summary>
        /// Обработать платёж по заказу.
        /// </summary>
        public async Task<(bool Success, string Message, decimal NewBalance)> ProcessPaymentAsync(int orderId, int userId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero");
            }

            if (!_accounts.TryGetValue(userId, out var account))
            {
                throw new InvalidOperationException("Account not found");
            }

            if (account.Balance < amount)
            {
                return (false, "Insufficient funds", account.Balance);
            }

            _ = _messageBroker.ProcessPaymentAsync(orderId, userId, amount);

            account.Balance -= amount;
            account.LastModifiedAt = DateTime.UtcNow;

            return (true, "Payment processing started", account.Balance);
        }
    }
} 