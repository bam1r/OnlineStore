using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    /// <summary>
    /// Контроллер для управления заказами пользователей.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с заказами.
        /// </summary>
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Создать заказ для пользователя.
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult> CreateOrder(
            [FromQuery] int userId,
            [FromQuery] string name,
            [FromQuery] decimal amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Order name cannot be empty");
                }

                if (amount <= 0)
                {
                    return BadRequest("Amount must be greater than 0");
                }

                var order = await _orderService.CreateOrderAsync(userId, name, amount);
                return CreatedAtAction(nameof(GetOrder), new { orderId = order.Id }, new {
                    orderId = order.Id,
                    userId = order.UserId,
                    name = order.Name,
                    amount = order.Amount,
                    status = order.Status.ToString(),
                    createdAt = order.CreatedAt,
                    lastModifiedAt = order.LastModifiedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить список заказов пользователя.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetUserOrders(int userId)
        {
            try
            {
                var orders = await _orderService.GetUserOrdersAsync(userId);
                var result = orders.Select(order => new {
                    orderId = order.Id,
                    userId = order.UserId,
                    name = order.Name,
                    amount = order.Amount,
                    status = order.Status.ToString(),
                    createdAt = order.CreatedAt,
                    lastModifiedAt = order.LastModifiedAt
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить заказ по идентификатору.
        /// </summary>
        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetOrder(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderAsync(orderId);
                return Ok(new {
                    orderId = order.Id,
                    userId = order.UserId,
                    name = order.Name,
                    amount = order.Amount,
                    status = order.Status.ToString(),
                    createdAt = order.CreatedAt,
                    lastModifiedAt = order.LastModifiedAt
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
    }
} 