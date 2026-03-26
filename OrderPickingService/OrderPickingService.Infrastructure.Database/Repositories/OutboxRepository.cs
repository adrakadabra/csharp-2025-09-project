using Microsoft.EntityFrameworkCore;
using OrderPickingService.Infrastructure.Database.Entities.Events;
using OrderPickingService.Services.Repositories.Abstractions;
using OrderPickingService.Services.Repositories.Abstractions.Dtos;

namespace OrderPickingService.Infrastructure.Database.Repositories;

internal sealed class OutboxRepository (DatabaseContext dbContext) : IOutboxRepository
{
    public async Task AddAsync(CreateOutboxMessageDto message, CancellationToken cancellationToken = default)
    {
        await dbContext.OutboxMessages.AddAsync(message.ToOutboxMessageEntity(), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<OutboxMessageDto>> GetUnprocessedAsync(int limit = 100, int maxAttemptsCount = 3, CancellationToken cancellationToken = default)
    {
        var entities = await dbContext.OutboxMessages
            .Where(message => message.ProcessedAt == null && message.Attempts <= maxAttemptsCount)
            .Take(limit)
            .ToListAsync(cancellationToken);
        
        return entities.Select(message => message.ToOutboxMessageDto()).ToList();
    }

    public async Task MarkAsProcessedAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.OutboxMessages
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken: cancellationToken);
        
        if (entity == null)
            throw new KeyNotFoundException($"Order with id {id} not found in database");
        
        entity.ProcessedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task IncrementAttemptsAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.OutboxMessages
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken: cancellationToken);
        
        if (entity == null)
            throw new KeyNotFoundException($"Order with id {id} not found in database");
        
        entity.Attempts++;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProcessedAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.OutboxMessages
            .FirstOrDefaultAsync(message => message.Id == id, cancellationToken: cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"Order with id {id} not found in database");
        
        dbContext.OutboxMessages.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}