namespace OrderService.Services
{
    /// <summary>
    /// Простой брокер сообщений для имитации асинхронной обработки заказов.
    /// </summary>
    public class SimpleMessageBroker : IMessageBroker
    {
        public SimpleMessageBroker() { }

        public async Task ProcessOrderAsync(int orderId, int userId, decimal amount)
        {
            // Simulate async processing
            await Task.Delay(100);
            Console.WriteLine($"Processing order {orderId} for user {userId} with amount {amount}");
        }
    }
} 