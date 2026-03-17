using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Application.Interfaces;

namespace OrdersService.Api.Controllers;

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
    public async Task<ActionResult<OrderDto>> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        try
        {
            var jwtToken = GetRawJwt();
            var order = await _ordersService.CreateOrderAsync(userId, jwtToken, request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        var order = await _ordersService.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return NotFound();

        if (order.UserId != userId)
            return Forbid();

        return Ok(order);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        var jwtToken = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(jwtToken))
            return Unauthorized();

        try
        {
            var cancelled = await _ordersService.CancelOrderAsync(id, userId, jwtToken, cancellationToken);

            if (!cancelled)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetMyOrders(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();

        if (userId is null)
            return Unauthorized();

        var orders = await _ordersService.GetUserOrdersAsync(userId, cancellationToken);
        return Ok(orders);
    }

    private string? GetUserIdFromClaims()
    {
        var rawValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(rawValue))
            return null;

        return rawValue;
    }

    private string GetRawJwt()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader))
            return string.Empty;

        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return authHeader["Bearer ".Length..].Trim();

        return authHeader.Trim();
    }
}