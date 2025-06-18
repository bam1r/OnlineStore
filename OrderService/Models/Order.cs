namespace OrderService.Models
{
    /// <summary>
    /// Модель заказа пользователя.
    /// </summary>
    public class Order
    {
        /// <summary>Идентификатор заказа.</summary>
        public int Id { get; set; }
        /// <summary>Идентификатор пользователя.</summary>
        public int UserId { get; set; }
        /// <summary>Название заказа.</summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>Сумма заказа.</summary>
        public decimal Amount { get; set; }
        /// <summary>Статус заказа.</summary>
        public OrderStatus Status { get; set; }
        /// <summary>Дата создания заказа.</summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>Дата последнего изменения заказа.</summary>
        public DateTime? LastModifiedAt { get; set; }
    }

    /// <summary>
    /// Перечисление статусов заказа.
    /// </summary>
    public enum OrderStatus
    {
        NEW,
        FINISHED,
        CANCELLED
    }
} 