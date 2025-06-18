using System;

namespace PaymentsService.Models
{
    /// <summary>
    /// Модель платёжного аккаунта пользователя.
    /// </summary>
    public class PaymentAccount
    {
        /// <summary>Идентификатор аккаунта (не возвращается клиенту).</summary>
        public int Id { get; set; }
        /// <summary>Идентификатор пользователя.</summary>
        public int UserId { get; set; }
        /// <summary>Баланс аккаунта.</summary>
        public decimal Balance { get; set; }
        /// <summary>Дата создания аккаунта.</summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>Дата последнего изменения аккаунта.</summary>
        public DateTime? LastModifiedAt { get; set; }
    }
} 