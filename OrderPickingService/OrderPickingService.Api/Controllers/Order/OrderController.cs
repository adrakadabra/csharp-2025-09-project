using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderPickingService.Api.Extensions;
using OrderPickingService.Services.Order.Abstractions;
using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Api.Controllers.Order;

[ApiController]
[Route("api/[controller]")]
public sealed class OrderController(
    IOrderService orderService,
    ILogger<OrderController> logger) : ControllerBase
{
    [HttpPost("CreateOrder")]
    [ProducesResponseType(typeof(CreateOrderDto),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(
        CreateOrderDto createOrderDto, 
        [FromServices] IValidator<CreateOrderDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(createOrderDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.GetErrors());
        }
        
        try
        {
            var result = await orderService.CreateOrderAsync(createOrderDto, cancellationToken);

            return Created($"/api/orders/{result.Id}", result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating order");
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
    
    [HttpGet("GetOrderByExternalId")]
    [ProducesResponseType(typeof(CreateOrderDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrderByExternalId(
        Guid externalOrderId, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await orderService.GetOrderByExternalId(externalOrderId, cancellationToken);

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogWarning(e,  "Order not found");
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}