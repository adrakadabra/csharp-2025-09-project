using Microsoft.EntityFrameworkCore;
using OrderPickingService.Domain.Entities;
using OrderPickingService.Infrastructure.Database.Entities.Picker;
using OrderPickingService.Infrastructure.Database.Entities.PickingSession;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.Database.Repositories;

internal sealed class PickingSessionRepository(DatabaseContext databaseContext) : IPickingSessionRepository
{
    public async Task<PickingSession?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var foundSession = await databaseContext.PickingSessions
            .Include(s => s.PickedItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(session => session.Id == id, cancellationToken);

        return foundSession?.ToPickingSession(); 
    }

    public async Task<PickingSession> CreateAsync(PickingSession session, CancellationToken cancellationToken)
    {
        var newSession = session.ToPickingSessionEntity();
        var result = await databaseContext.PickingSessions.AddAsync(newSession, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        return newSession.ToPickingSession();
    }

    public async Task UpdateAsync(PickingSession session, CancellationToken cancellationToken)
    {
        var entity = await databaseContext.PickingSessions
                .Include(s => s.PickedItems)
                .FirstOrDefaultAsync(s => s.Id == session.Id, cancellationToken);
        
        if(entity == null)
            throw new KeyNotFoundException($"Session with id {session.Id} not found in database");
        
        entity.FinishedAt = session.FinishedAt;
        entity.PickingStatus = session.PickingStatus;
        entity.Notes = session.Notes;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = "system";

        var newItems = session.PickedItems
            .Where(item => item.Id == 0)
            .Select(item => item.ToPickedItemEntity());
        
        entity.PickedItems.AddRange(newItems);
        
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}