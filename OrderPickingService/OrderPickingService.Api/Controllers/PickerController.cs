using Microsoft.AspNetCore.Mvc;
using OrderPickingService.Services.Picker.Abstractions;
using OrderPickingService.Services.Picker.Dtos;

namespace OrderPickingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PickerController(IPickerService pickerService) : ControllerBase
{
        
    [HttpGet("GetPickers")]
    public async Task<IActionResult> GetPickers()
    {
        var result = await pickerService.GetAllPickersAsync();
        return Ok(result);
    }
    
    [HttpGet("GetPickerById")]
    public async Task<IActionResult> GetPickerById(int id)
    {
        var result = await pickerService.GetPickerByIdAsync(id);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpPost("CreatePiker")]
    [ProducesResponseType(typeof(PickerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePicker(CreatePickerDto pickerDto)
    {
        try
        {
            var result = await pickerService.CreatePickerAsync(pickerDto);
                
            return CreatedAtAction(
                nameof(GetPickerById),
                new { id = result.Id }, 
                result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //TODO: add logging
            return StatusCode(StatusCodes.Status400BadRequest, "failed to create picker");
        }
    }
    
    [HttpPost("UpdatePicker")]
    [ProducesResponseType(typeof(PickerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePicker(long id, UpdatePikerDto updatePikerDto)
    {
        try
        {
            var result = await pickerService.UpdatePikerAsync(id, updatePikerDto);
                
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e); //TODO: add logging
            return StatusCode(StatusCodes.Status400BadRequest, "failed to update picker");
        }
    }
}