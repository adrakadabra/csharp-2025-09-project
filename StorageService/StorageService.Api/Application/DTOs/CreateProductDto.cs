namespace StorageService.Api.Application.DTOs;

public record CreateProductDto(
    string Name,
    string? Description,
    decimal Price,
    int Quantity,
    string CategoryName,
    string ManufacturerName,
    string SectionCode
);
