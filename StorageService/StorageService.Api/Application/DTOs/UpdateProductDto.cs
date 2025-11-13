
using System.ComponentModel.DataAnnotations;

namespace StorageService.Api.Application.DTOs;

public class UpdateProductDto
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}
