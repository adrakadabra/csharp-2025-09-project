using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderPickingService.Api.Extensions;
using OrderPickingService.Services.Picker.Abstractions;
using OrderPickingService.Services.Picker.Dtos;

namespace OrderPickingService.Api.Controllers.Picker;

[ApiController]
[Route("api/[controller]")]
public sealed class PickerController(IPickerService pickerService) : ControllerBase
{
        
    [HttpGet("GetPickers")]
    public async Task<IActionResult> GetPickers(CancellationToken cancellationToken)
    {
        var result = await pickerService.GetAllPickersAsync(cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("GetPickerById")]
    public async Task<IActionResult> GetPickerById(
        long id, 
        [FromServices] IValidator<long> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(id, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.GetErrors());
        }
        
        var result = await pickerService.GetPickerByIdAsync(id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpPost("CreatePiker")]
    [ProducesResponseType(typeof(PickerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePicker(
        CreatePickerDto pickerDto,
        [FromServices] IValidator<CreatePickerDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(pickerDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.GetErrors());
        }
        
        try
        {
            var result = await pickerService.CreatePickerAsync(pickerDto, cancellationToken);
                
            return CreatedAtAction(
                nameof(GetPickerById),
                new { id = result.Id }, 
                result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //TODO: add logging
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
    
    [HttpPost("UpdatePicker")]
    [ProducesResponseType(typeof(PickerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePicker(
        UpdatePickerDto updatePickerDto,
        [FromServices] IValidator<UpdatePickerDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(updatePickerDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.GetErrors());
        }
        
        try
        {
            var result = await pickerService.UpdatePikerAsync(updatePickerDto, cancellationToken);
                
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //TODO: add logging
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}