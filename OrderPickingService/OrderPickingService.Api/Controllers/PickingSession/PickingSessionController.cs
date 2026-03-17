using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderPickingService.Api.Extensions;
using OrderPickingService.Services.Picking.Abstractions;
using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Api.Controllers.PickingSession;

[ApiController]
[Route("api/[controller]")]
public sealed class PickingSessionController(IPickingService pickingService) : ControllerBase
{
    [HttpPost("ClaimOrder")]
    [ProducesResponseType(typeof(CreatedPickingSessionDto),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ClaimOrder(
        ClaimOrderDto claimOrderDto,
        [FromServices] IValidator<ClaimOrderDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(claimOrderDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.GetErrors());
        }
        
        try
        {
            var result = await pickingService.ClaimOrder(claimOrderDto, cancellationToken);

            return Created($"/api/pickingsession/{result.IdSession}", result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //TODO: add logging
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}