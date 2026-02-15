using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderPickingService.Api.Extensions;
using OrderPickingService.Services.Order.Abstractions;
using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Api.Controllers.Order;

[ApiController]
[Route("api/[controller]")]
public sealed class OrderController(IOrderService orderService) : ControllerBase
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
            Console.WriteLine(e); //TODO: add logging
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}