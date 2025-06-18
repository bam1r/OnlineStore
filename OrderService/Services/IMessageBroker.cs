namespace OrderService.Services
{
    /// <summary>
    /// Интерфейс для асинхронной обработки заказов через брокер сообщений.
    /// </summary>
    public interface IMessageBroker
    {
        Task ProcessOrderAsync(int orderId, int userId, decimal amount);
    }
} 