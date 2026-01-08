using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderPickingService.Infrastructure.Database;
using OrderPickingService.Services.Picker.Abstractions;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(IPickerService pickerService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult GetProducts()
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        var userId = User.FindFirst("sub")?.Value;
        var username = User.FindFirst("preferred_username")?.Value;
        
        return Ok(new 
        {
            Message = "Это защищенный endpoint!",
            UserId = userId,
            Username = username,
            Roles = roles,
            Products = new[] 
            {
                new { Id = 1, Product = "Молоко", Price = 120 },
                new { Id = 2, Product = "Хлеб", Price = 50 }
            }
        });
    }
    
    [HttpGet("admin")]
    [Authorize(Roles = "admin")]
    public IActionResult GetAdminData()
    {
        return Ok(new { Message = "Секретные данные только для админов" });
    }
    
    [HttpGet("customer")]
    [Authorize(Roles = "customer")]
    public IActionResult GetCustomerData()
    {
        return Ok(new { Message = "Данные для заказчиков" });
    }
    
    [HttpGet("GetPickers")]
    public async Task<IActionResult> GetPickers()
    {
        var result = await pickerService.GetPickers();
        return Ok(result);
    }
}