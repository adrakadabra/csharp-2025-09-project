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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    [HttpPost("PickItem")]
    [ProducesResponseType(typeof(PickItemResultDto),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PickItem(
        PickItemDto pickItemDto,
        [FromServices] IValidator<PickItemDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(pickItemDto, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.GetErrors());
        }
        
        try
        {
            var result = await pickingService.PickItemAsync(pickItemDto, cancellationToken);

            if (result.Success)
            {
                return Created($"/api/pickedItem/{result.Item?.Id}", result);
            }
            
            return StatusCode(StatusCodes.Status400BadRequest, result.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //TODO: add logging
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    [HttpGet("GetPickingSessionById")]
    [ProducesResponseType(typeof(PickingSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPickingSessionById(
        long id, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await pickingService.GetPickingSessionByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}