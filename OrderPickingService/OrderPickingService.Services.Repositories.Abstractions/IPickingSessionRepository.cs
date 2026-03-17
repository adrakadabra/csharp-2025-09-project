using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IPickingSessionRepository
{
    Task<PickingSession?> GetByIdAsync(long id, CancellationToken cancellationToken);

    Task<PickingSession> CreateAsync(PickingSession session, CancellationToken cancellationToken);

    Task UpdateAsync(PickingSession session, CancellationToken cancellationToken);
}