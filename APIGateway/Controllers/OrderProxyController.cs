using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    /// <summary>
    /// Прокси-контроллер для взаимодействия с OrderService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderProxyController : ControllerBase
    {
        /// <summary>HTTP-клиент для отправки запросов к OrderService.</summary>
        private readonly HttpClient _httpClient;
        /// <summary>Базовый URL OrderService.</summary>
        private readonly string _orderServiceBaseUrl;

        public OrderProxyController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _orderServiceBaseUrl = configuration["Services:OrderService"];
        }

        /// <summary>
        /// Проксировать создание заказа.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromQuery] int userId, [FromQuery] string name, [FromQuery] decimal amount)
        {
            var url = $"{_orderServiceBaseUrl}/api/Order/create?userId={userId}&name={name}&amount={amount}";
            var response = await _httpClient.PostAsync(url, null);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
        }

        /// <summary>
        /// Проксировать получение заказов пользователя.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var url = $"{_orderServiceBaseUrl}/api/Order/user/{userId}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
        }

        /// <summary>
        /// Проксировать получение заказа по идентификатору.
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var url = $"{_orderServiceBaseUrl}/api/Order/{orderId}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
        }
    }
} 