using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Api.DTO;
using OrdersService.Api.Services;
using System.Security.Claims;

namespace OrdersService.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request)
        {
            int userId = GetUserIdFromClaims();
            string jwtToken = GetRawJwt();

            var order = await _ordersService.CreateOrderAsync(userId, jwtToken, request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _ordersService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
        {
            int userId = GetUserIdFromClaims();
            var orders = await _ordersService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        private int GetUserIdFromClaims()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            return int.Parse(claim.Value);
        }

        private string GetRawJwt()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader))
                return string.Empty;

            if (authHeader.StartsWith("Bearer "))
                return authHeader.Substring("Bearer ".Length);

            return authHeader;
        }
    }
}