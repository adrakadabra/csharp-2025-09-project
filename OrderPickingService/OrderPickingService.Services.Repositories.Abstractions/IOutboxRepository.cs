using OrderPickingService.Services.Repositories.Abstractions.Dtos;

namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IOutboxRepository
{
    Task AddAsync(CreateOutboxMessageDto message, CancellationToken cancellationToken = default);
    Task<List<OutboxMessageDto>> GetUnprocessedAsync(int limit = 100, int maxAttemptsCount = 3, CancellationToken cancellationToken = default);
    Task IncrementAttemptsAsync(long id, CancellationToken cancellationToken = default);
    Task DeleteProcessedAsync(long id, CancellationToken cancellationToken = default);
}