
using System.ComponentModel.DataAnnotations;

namespace StorageService.Api.Application.DTOs;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
