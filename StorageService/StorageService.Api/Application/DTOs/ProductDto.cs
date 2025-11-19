
using System;

namespace StorageService.Api.Application.DTOs;

public record ProductDto(Guid Id, string Name, string? Description, int Quantity, decimal Price, DateTime CreatedAt, DateTime UpdatedAt);
