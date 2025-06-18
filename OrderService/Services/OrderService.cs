using System.Collections.Concurrent;
using OrderService.Models;

namespace OrderService.Services
{
    /// <summary>
    /// Сервис для управления заказами пользователей.
    /// </summary>
    public class OrderService : IOrderService
    {
        /// <summary>Хранилище заказов пользователей.</summary>
        private static readonly ConcurrentDictionary<int, Order> _orders = new();
        /// <summary>Следующий идентификатор заказа.</summary>
        private static int _nextId = 1;
        /// <summary>Брокер сообщений для асинхронной обработки заказов.</summary>
        private readonly IMessageBroker _messageBroker;

        public OrderService(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        /// <summary>
        /// Создать заказ для пользователя.
        /// </summary>
        public async Task<Order> CreateOrderAsync(int userId, string name, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Order name cannot be empty", nameof(name));
            }

            var order = new Order
            {
                Id = _nextId++,
                UserId = userId,
                Name = name,
                Amount = amount,
                Status = OrderStatus.NEW,
                CreatedAt = DateTime.UtcNow
            };

            _orders.TryAdd(order.Id, order);

            _ = _messageBroker.ProcessOrderAsync(order.Id, userId, amount);

            return order;
        }

        /// <summary>
        /// Получить список заказов пользователя.
        /// </summary>
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return _orders.Values.Where(o => o.UserId == userId);
        }

        /// <summary>
        /// Получить заказ по идентификатору.
        /// </summary>
        public async Task<Order> GetOrderAsync(int orderId)
        {
            if (!_orders.TryGetValue(orderId, out var order))
            {
                throw new InvalidOperationException("Order not found");
            }

            return order;
        }

        /// <summary>
        /// Обновить статус заказа.
        /// </summary>
        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            if (!_orders.TryGetValue(orderId, out var order))
            {
                throw new InvalidOperationException("Order not found");
            }

            order.Status = status;
            order.LastModifiedAt = DateTime.UtcNow;
        }
    }
} 