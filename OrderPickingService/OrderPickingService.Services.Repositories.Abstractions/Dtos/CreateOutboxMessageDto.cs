namespace OrderPickingService.Services.Repositories.Abstractions.Dtos;

public record CreateOutboxMessageDto(
    string EventType,
    string Message);