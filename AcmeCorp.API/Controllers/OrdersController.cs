using AcmeCore.Infrastructure.Models;
using AcmeCore.Service.Services.Interfaces;
using AcmeCorp.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderViewModel>> GetOrderByIdAsync(long orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderViewModel>> CreateOrderAsync(CreateOrderViewModel createOrderViewModel)
        {
            var createdOrder = await _orderService.CreateOrderAsync(createOrderViewModel);
            return CreatedAtAction(nameof(GetOrderByIdAsync), new { orderId = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderAsync(long orderId, UpdateOrderViewModel updateOrderViewModel)
        {
            if (orderId != updateOrderViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                await _orderService.UpdateOrderAsync(updateOrderViewModel);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrderAsync(long orderId)
        {
            try
            {
                await _orderService.DeleteOrderAsync(orderId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
