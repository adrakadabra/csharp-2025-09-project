namespace StorageService.Api.Application.DTOs;

public record CreateProductDto(
    string Name,
    string Article,
    string? Description,
    decimal Price,
    int Quantity,
    string CategoryName,
    string ManufacturerName,
    string SectionCode
);
