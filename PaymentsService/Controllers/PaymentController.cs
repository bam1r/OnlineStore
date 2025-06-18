using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentsService.Models;
using PaymentsService.Services;

namespace PaymentsService.Controllers
{
    /// <summary>
    /// Контроллер для управления платёжными аккаунтами пользователей.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с платёжными аккаунтами.
        /// </summary>
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Создать платёжный аккаунт для пользователя.
        /// </summary>
        [HttpPost("accounts/{userId}")]
        public async Task<ActionResult> CreateAccount(int userId)
        {
            try
            {
                var account = await _paymentService.CreateAccountAsync(userId);
                return Ok(new {
                    userId = account.UserId,
                    balance = account.Balance,
                    createdAt = account.CreatedAt,
                    lastModifiedAt = account.LastModifiedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить платёжный аккаунт пользователя.
        /// </summary>
        [HttpGet("accounts/{userId}")]
        public async Task<ActionResult> GetAccount(int userId)
        {
            try
            {
                var account = await _paymentService.GetAccountAsync(userId);
                return Ok(new {
                    userId = account.UserId,
                    balance = account.Balance,
                    createdAt = account.CreatedAt,
                    lastModifiedAt = account.LastModifiedAt
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Пополнить баланс платёжного аккаунта пользователя.
        /// </summary>
        [HttpPost("accounts/{userId}/topup")]
        public async Task<ActionResult<object>> TopUpAccount(int userId, [FromBody] decimal amount)
        {
            try
            {
                var (success, message, newBalance) = await _paymentService.TopUpAccountAsync(userId, amount);
                return Ok(new { success, message, newBalance });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 