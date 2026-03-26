namespace OrderPickingService.Services.Repositories.Abstractions.Dtos;

public record OutboxMessageDto(
    long Id,
    string EventType,
    string Message,
    DateTime? ProcessedAt,
    int Attempts);