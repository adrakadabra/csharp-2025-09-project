using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.Outbox;

internal sealed class OutboxMessageBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxMessageBackgroundService> logger) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(1);
    private readonly int _batchSize = 100;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing outbox messages");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var mapper = scope.ServiceProvider.GetRequiredService<IOutboxMessageMapper>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messages = await outboxRepository.GetUnprocessedAsync(
            limit: _batchSize,
            cancellationToken: cancellationToken);
        
        foreach (var message in messages)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var request = mapper.Map(message);
                var result = await mediator.Send(request, cancellationToken);

                if (result.Success)
                {
                    await outboxRepository.DeleteProcessedAsync(message.Id, cancellationToken);
                }
                else
                {
                    await outboxRepository.IncrementAttemptsAsync(message.Id, cancellationToken);
                }
                await unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                logger.LogError(e, $"Error processing outbox message {message.Id}");
            }
        }
    }
}