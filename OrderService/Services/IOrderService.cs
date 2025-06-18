using OrderService.Models;

namespace OrderService.Services
{
    /// <summary>
    /// Интерфейс сервиса для управления заказами пользователей.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>Создать заказ для пользователя.</summary>
        Task<Order> CreateOrderAsync(int userId, string name, decimal amount);
        /// <summary>Получить список заказов пользователя.</summary>
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        /// <summary>Получить заказ по идентификатору.</summary>
        Task<Order> GetOrderAsync(int orderId);
        /// <summary>Обновить статус заказа.</summary>
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
} 