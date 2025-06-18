using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    /// <summary>
    /// Прокси-контроллер для взаимодействия с PaymentsService.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsProxyController : ControllerBase
    {
        /// <summary>HTTP-клиент для отправки запросов к PaymentsService.</summary>
        private readonly HttpClient _httpClient;
        /// <summary>Базовый URL PaymentsService.</summary>
        private readonly string _paymentsServiceBaseUrl;

        public PaymentsProxyController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _paymentsServiceBaseUrl = configuration["Services:PaymentsService"];
        }

        /// <summary>
        /// Проксировать создание платёжного аккаунта.
        /// </summary>
        [HttpPost("accounts/{userId}")]
        public async Task<IActionResult> CreateAccount(int userId)
        {
            var url = $"{_paymentsServiceBaseUrl}/api/Payment/accounts/{userId}";
            var response = await _httpClient.PostAsync(url, null);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
        }

        /// <summary>
        /// Проксировать получение платёжного аккаунта.
        /// </summary>
        [HttpGet("accounts/{userId}")]
        public async Task<IActionResult> GetAccount(int userId)
        {
            var url = $"{_paymentsServiceBaseUrl}/api/Payment/accounts/{userId}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
        }

        /// <summary>
        /// Проксировать пополнение баланса платёжного аккаунта.
        /// </summary>
        [HttpPost("accounts/{userId}/topup")]
        public async Task<IActionResult> TopUpAccount(int userId, [FromBody] decimal amount)
        {
            var url = $"{_paymentsServiceBaseUrl}/api/Payment/accounts/{userId}/topup";
            var contentBody = new StringContent(amount.ToString(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, contentBody);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
        }
    }
} 