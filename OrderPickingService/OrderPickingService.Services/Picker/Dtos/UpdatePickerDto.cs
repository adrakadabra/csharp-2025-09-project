namespace OrderPickingService.Services.Picker.Dtos;

public sealed record UpdatePickerDto(
    long id,
    string? FirstName,
    string? LastName);